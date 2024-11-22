using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarrierLayer.Db;
using BarrierLayer.Domain.Dto;
using BarrierLayer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BarrierLayer.Services
{
    public class UserService(DomainContext db, ConfigService config)
    {
        public async Task<Guid> CreateUser(string number, string password)
        {
            number = number.FormatToNumber();
            if (!number.ValidateNumber()) throw new ArgumentException("Плохой номер телефона");
            await config.VerifyPassword(password);
            var user = new User()
            {
                Number = number,
                Token = Guid.NewGuid(),
                Status = UserStatus.Registering
            };
            db.Users.AddOrUpdate(user, x => x.Number == user.Number);
            await db.SaveChangesAsync();
            return user.Token;
        }

        public async Task<Guid> RegisterApp(string number, Guid token)
        {
            number = number.FormatToNumber();
            if (!number.ValidateNumber()) throw new ArgumentException("Плохой номер телефона");
            var user = await db.Users.FirstOrDefaultAsync(x => x.Number == number);
            if (user == null || !user.Token.Equals(token))
                throw new ArgumentException("Пользователь не зарегистрирован");
            user.Token = Guid.NewGuid();
            user.Status = UserStatus.Works;
            db.Update(user);
            await db.SaveChangesAsync();
            return user.Token;
        }

        public async Task<List<BarrierForUserDto>> GetBarrierList(Guid userKey)
        {
            return (await db.GetUserBarriers(userKey))
                .Select(b => new BarrierForUserDto() {Id = b.Id, Address = b.Address}).ToList();
        }

        public async Task<List<BarrierDto>> GetBarrierList(string password)
        {
            await config.VerifyPassword(password);
            var barriers = await db.Barriers.ToListAsync();
            return barriers.Select(b => new BarrierDto {Id = b.Id, Number = b.BarrierNumber, Address = b.Address})
                .ToList();
        }

        public async Task AddBarrierToUser(int barrierId, string userNumber)
        {
            userNumber = userNumber.FormatToNumber();
            if (!userNumber.ValidateNumber()) throw new ArgumentException("Плохой номер телефона");
            var user = await db.GetUserByNumber(userNumber);
            var barrier = await db.Barriers.FirstOrDefaultAsync(b => b.Id == barrierId);
            if (barrier == null) throw new ArgumentException("Нет такого барьера");
            user.Barriers.Add(new UserBarrier() {BarrierId = barrier.Id, UserId = user.Id});
            db.Update(user);
            await db.SaveChangesAsync();
        }
    }
}