using System;
using System.Threading.Tasks;
using BarrierLayer.Dto;
using BarrierLayer.Helpers;
using BarrierLayer.Models;

namespace BarrierLayer.Services
{
    public class GuestBarrierService
    {
        private readonly BarrierService _barrierService;
        private readonly DomainContext _domainContext;
        private readonly ConfigService _configService;

        public GuestBarrierService(DomainContext domainContext, BarrierService barrierService,
            ConfigService configService)
        {
            _configService = configService;
            _domainContext = domainContext;
            _barrierService = barrierService;
        }

        public async Task<Guest> AddGuest(int barrierId, DateTime expires, string password)
        {
            if (IsExpired(expires))
                throw new ArgumentException($"Время {expires} уже прошло");
            await _configService.VerifyPassword(password);
            
            var barrier = await _domainContext.GetBarrierById(barrierId)
                .ThrowIfNull(barrierId);
            var guest = new Guest()
            {
                Id = Guid.NewGuid(),
                Barrier = barrier,
                Expires = expires
            };
            await _domainContext.AddAsync(guest);
            await _domainContext.SaveChangesAsync();
            return guest;
        }

        public async Task<Guest> ChangeGuestExpiration(Guid guestId, DateTime expires, string password)
        {
            if(IsExpired(expires))
                throw new ArgumentException($"Время {expires} уже прошло");
            await _configService.VerifyPassword(password);
            
            var guest = await _domainContext.Guest_ById(guestId).ThrowIfNull(guestId);
            guest.Expires = expires;
            await _domainContext.SaveChangesAsync();
            return guest;
        }

        public Task<Guest> GetGuest(Guid guestId) => _domainContext.Guest_ById(guestId).ThrowIfNull(guestId);

        public async Task<Guest> OpenBarrier(Guid guestId)
        {
            var guest = await _domainContext.Guest_ById(guestId).ThrowIfNull(guestId);
            if(IsExpired(guest.Expires))
                throw new ArgumentException($"Время действия гостевого доступа закончилось {guest.Expires}");
            await _barrierService.Open(guest.Barrier);
            return guest;
        }

        private static bool IsExpired(DateTime date) => date < DateTime.Now;
    }
}