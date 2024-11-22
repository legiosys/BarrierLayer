using System;
using System.Threading.Tasks;
using BarrierLayer.Db;
using BarrierLayer.Domain.Models;
using BarrierLayer.Helpers;

namespace BarrierLayer.Services
{
    public class GuestBarrierService(
        DomainContext domainContext,
        BarrierService barrierService,
        ConfigService configService)
    {
        public async Task<Guest> AddGuest(int barrierId, DateTime expires, string password)
        {
            if (IsExpired(expires))
                throw new ArgumentException($"Время {expires} уже прошло");
            await configService.VerifyPassword(password);

            var barrier = await domainContext.GetBarrierById(barrierId)
                .ThrowIfNull(barrierId);
            var guest = new Guest()
            {
                Id = Guid.NewGuid(),
                Barrier = barrier,
                Expires = expires.ToUniversalTime()
            };
            await domainContext.AddAsync(guest);
            await domainContext.SaveChangesAsync();
            return guest;
        }

        public async Task<Guest> ChangeGuestExpiration(Guid guestId, DateTime expires, string password)
        {
            if (IsExpired(expires))
                throw new ArgumentException($"Время {expires} уже прошло");
            await configService.VerifyPassword(password);

            var guest = await domainContext.Guest_ById(guestId).ThrowIfNull(guestId);
            guest.Expires = expires;
            await domainContext.SaveChangesAsync();
            return guest;
        }

        public Task<Guest> GetGuest(Guid guestId) => domainContext.Guest_ById(guestId).ThrowIfNull(guestId);

        public async Task<Guest> OpenBarrier(Guid guestId)
        {
            var guest = await domainContext.Guest_ById(guestId).ThrowIfNull(guestId);
            if (IsExpired(guest.Expires))
                throw new ArgumentException($"Время действия гостевого доступа закончилось {guest.Expires}");
            await barrierService.Open(guest.Barrier);
            return guest;
        }

        private static bool IsExpired(DateTime date) => date < DateTime.Now;
    }
}