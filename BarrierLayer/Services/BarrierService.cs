using System;
using System.Linq;
using System.Threading.Tasks;
using BarrierLayer.Barriers;
using BarrierLayer.Db;
using BarrierLayer.Domain.Dto;
using BarrierLayer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BarrierLayer.Services
{
    public class BarrierService(DomainContext db, BarrierFacadeFactory barrierFactory)
    {
        private async Task<Barrier> GetBarrier(Guid userKey, int barrierId)
        {
            var user = await db.GetUserByToken(userKey);
            if (user == null) throw new ArgumentException("Пользователь не найден");
            var barrier = user.Barriers.FirstOrDefault(b => b.BarrierId == barrierId);
            if (barrier == null) throw new ArgumentException("Шлагбаум не найден");
            return await db.GetBarrierById(barrierId);
        }

        public async Task Open(Guid userKey, int barrierId)
        {
            var barrier = await GetBarrier(userKey, barrierId);
            await Open(barrier);
        }

        public async Task Open(Barrier barrier)
        {
            var barrierFacade = barrierFactory.Create(barrier);
            await barrierFacade.Open();
        }

        public async Task<BarrierAddResult> Register(string userNumber, string barrierNumber, BarrierType type)
        {
            userNumber = userNumber.FormatToNumber();
            barrierNumber = barrierNumber.FormatToNumber();
            var analog =
                await db.Barriers.FirstOrDefaultAsync(b => b.BarrierType == type && b.UserNumber.Equals(userNumber));
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
                var facade = barrierFactory.Create(barrier);
                await facade.Register(userNumber);
            }

            db.Add(barrier);
            await db.SaveChangesAsync();
            return new BarrierAddResult()
            {
                Id = barrier.Id,
                Status = (analog == null) ? BarrierAddStatus.WaitForConfirmation : BarrierAddStatus.Confirmed
            };
        }

        public async Task<BarrierAddResult> Confirm(int barrierId, string smsCode)
        {
            var barrier = await db.Barriers.FirstOrDefaultAsync(b => b.Id == barrierId);
            if (barrier == null) return new BarrierAddResult() {Id = barrierId, Status = BarrierAddStatus.Error};
            if (string.IsNullOrWhiteSpace(barrier.Token))
            {
                var facade = barrierFactory.Create(barrier);
                var result = await facade.Confirm(barrier.UserNumber, smsCode);
                if (string.IsNullOrWhiteSpace(result.Key))
                    return new BarrierAddResult() {Id = barrierId, Status = BarrierAddStatus.Error};
                barrier.Token = result.Key;
                db.Update(barrier);
                await db.SaveChangesAsync();
            }

            return new BarrierAddResult()
            {
                Id = barrier.Id,
                Status = BarrierAddStatus.Confirmed
            };
        }

        public async Task<BarrierAddResult> AddManual(string userNumber, string barrierNumber, BarrierType type,
            string token)
        {
            userNumber = userNumber.FormatToNumber();
            barrierNumber = barrierNumber.FormatToNumber();
            var barrier = new Barrier()
            {
                BarrierNumber = barrierNumber,
                UserNumber = userNumber,
                BarrierType = type,
                Token = token
            };
            db.Add(barrier);
            await db.SaveChangesAsync();
            return new BarrierAddResult()
            {
                Id = barrier.Id,
                Status = BarrierAddStatus.Confirmed
            };
        }
    }
}