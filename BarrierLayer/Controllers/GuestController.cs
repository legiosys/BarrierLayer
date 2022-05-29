using System;
using System.Threading.Tasks;
using BarrierLayer.Dto;
using BarrierLayer.LibCandidates.Vault;
using BarrierLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BarrierLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestController : ControllerBase
    {
        private readonly GuestBarrierService _guestBarrierService;
        private readonly IOptionsSnapshot<BarrierSettings> _dbSettings;

        public GuestController(GuestBarrierService guestBarrierService, IOptionsSnapshot<BarrierSettings> dbSettings)
        {
            _dbSettings = dbSettings;
            _guestBarrierService = guestBarrierService;
        }

        [HttpPost("[action]")]
        public async Task<GuestDto> Add(int barrierId, DateTime expires, string password)
            => await _guestBarrierService.AddGuest(barrierId, expires, password);


        [HttpPost("[action]")]
        public async Task<GuestDto> ChangeExpiration(Guid guestId, DateTime expires, string password)
            => await _guestBarrierService.ChangeGuestExpiration(guestId, expires, password);

        [HttpGet("{guestId:guid}")]
        public async Task<GuestDto> GetInfo(Guid guestId)
            => await _guestBarrierService.GetGuest(guestId);

        [HttpPost("{guestId:guid}")]
        public async Task<GuestDto> OpenBarrier(Guid guestId)
            => await _guestBarrierService.OpenBarrier(guestId);

        [HttpGet("afaf")]
        public BarrierSettings GetSettings() => _dbSettings.Value;
    }
}