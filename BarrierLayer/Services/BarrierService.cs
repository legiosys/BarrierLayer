using BarrierLayer.Barriers;
using BarrierLayer.Dto;
using BarrierLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace BarrierLayer.Services
{
    public class BarrierService
    {
        private readonly DomainContext _db;
        private readonly BarrierFacadeFactory _barrierFactory;

        public BarrierService(DomainContext db, BarrierFacadeFactory barrierFactory)
        {
            _db = db;
            _barrierFactory = barrierFactory;
        }
        private async Task<IBarrierFacade> GetBarrier(Guid userKey, int barrierId)
        {
            var user = await _db.GetUserByToken(userKey);
            if (user == null) throw new ArgumentException("Пользователь не найден");
            var barrier = user.Barriers.FirstOrDefault(b => b.BarrierId == barrierId);
            if (barrier == null) throw new ArgumentException("Шлагбаум не найден");
            return _barrierFactory.Create(await _db.GetBarrierById(barrierId));
        }

        public async Task Open(Guid userKey, int barrierId)
        {
            var barrier = await GetBarrier(userKey, barrierId);
            await barrier.Open();
        }
        public async Task<BarrierAddResult> Register(string userNumber, string barrierNumber, BarrierType type)
        {
            var analog = await _db.Barriers.FirstOrDefaultAsync(b => b.BarrierType == type && b.UserNumber.Equals(userNumber));
            var barrier = new Barrier()
            {
                BarrierNumber = barrierNumber,
                BarrierType = type,
                UserNumber = userNumber,
            };
            if (analog != null)
                barrier.Token = analog.Token;
            else
            {
                var facade = _barrierFactory.Create(barrier);
                await facade.Register(userNumber);
            }
            _db.Add(barrier);
            await _db.SaveChangesAsync();
            return new BarrierAddResult()
            {
                Id = barrier.Id,
                Status = (analog == null) ? BarrierAddStatus.WaitForConfirmation : BarrierAddStatus.Confirmed
            };

        }
        public async Task<BarrierAddResult> Confirm(int barrierId, string smsCode)
        {
            var barrier = await _db.Barriers.FirstOrDefaultAsync(b => b.Id == barrierId);
            if (barrier == null) return new BarrierAddResult() { Id = barrierId, Status = BarrierAddStatus.Error };
            if (string.IsNullOrWhiteSpace(barrier.Token))
            {
                var facade = _barrierFactory.Create(barrier);
                var result = await facade.Confirm(barrier.UserNumber,smsCode);
                if (string.IsNullOrWhiteSpace(result.Key))
                    return new BarrierAddResult() { Id = barrierId, Status = BarrierAddStatus.Error };
                barrier.Token = result.Key;
                _db.Update(barrier);
                await _db.SaveChangesAsync();
            }
            return new BarrierAddResult()
            {
                Id = barrier.Id,
                Status = BarrierAddStatus.Confirmed
            };
        }
        public async Task<BarrierAddResult> AddManual(string userNumber, string barrierNumber, BarrierType type, string token)
        {
            var barrier = new Barrier()
            {
                BarrierNumber = barrierNumber,
                UserNumber = userNumber,
                BarrierType = type,
                Token = token
            };
            _db.Add(barrier);
            await _db.SaveChangesAsync();
            return new BarrierAddResult()
            {
                Id = barrier.Id,
                Status = BarrierAddStatus.Confirmed
            };
        }
    }
}
