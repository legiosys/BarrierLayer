using BarrierLayer.Dto;
using BarrierLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace BarrierLayer.Services
{
    public class UserService
    {
        private readonly DomainContext _db;
        private readonly ConfigService _config;

        public UserService(DomainContext db, ConfigService config)
        {
            _db = db;
            _config = config;
        }

        public async Task<Guid> CreateUser(string number, string password)
        {
            number = number.FormatToNumber();
            if (!number.ValidateNumber()) throw new ArgumentException("Плохой номер телефона");
            await _config.VerifyPassword(password);
            var user = new User()
            {
                Number = number,
                Token = Guid.NewGuid(),
                Status = UserStatus.Registering
            };
            _db.Users.AddOrUpdate(user, x => x.Number == user.Number);
            await _db.SaveChangesAsync();
            return user.Token;
        }

        public async Task<Guid> RegisterApp(string number, Guid token)
        {
            number = number.FormatToNumber();
            if (!number.ValidateNumber()) throw new ArgumentException("Плохой номер телефона");
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Number == number);
            if (user == null || !user.Token.Equals(token))
                throw new ArgumentException("Пользователь не зарегистрирован");
            user.Token = Guid.NewGuid();
            user.Status = UserStatus.Works;
            _db.Update(user);
            await _db.SaveChangesAsync();
            return user.Token;
        }

        public async Task<List<int>> GetBarrierList(Guid userKey)
        {
            var user = await _db.GetUserByToken(userKey);
            return user.Barriers?.Select(b => b.BarrierId).ToList();
        }
        public async Task<List<BarrierDto>> GetBarrierList(string password)
        {
            await _config.VerifyPassword(password);
            var barriers = await _db.Barriers.ToListAsync();
            return barriers.Select(b => new BarrierDto { Id = b.Id, Number = b.BarrierNumber }).ToList();
        }

        public async Task AddBarrierToUser(int barrierId, string userNumber)
        {
            userNumber = userNumber.FormatToNumber();
            if (!userNumber.ValidateNumber()) throw new ArgumentException("Плохой номер телефона");
            var user = await _db.GetUserByNumber(userNumber);
            var barrier = await _db.Barriers.FirstOrDefaultAsync(b => b.Id == barrierId);
            if (barrier == null) throw new ArgumentException("Нет такого барьера");
            user.Barriers.Add(new UserBarrier() { BarrierId = barrier.Id, UserId = user.Id });
            _db.Update(user);
            await _db.SaveChangesAsync();
        }
    }
}
