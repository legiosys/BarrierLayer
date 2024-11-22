using System;
using System.Threading.Tasks;
using BarrierLayer.Domain.Dto;
using BarrierLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarrierLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestController(GuestBarrierService guestBarrierService) : ControllerBase
    {
        [HttpPost("[action]")]
        public async Task<GuestDto> Add(int barrierId, DateTime expires, string password)
            => await guestBarrierService.AddGuest(barrierId, expires, password);


        [HttpPost("[action]")]
        public async Task<GuestDto> ChangeExpiration(Guid guestId, DateTime expires, string password)
            => await guestBarrierService.ChangeGuestExpiration(guestId, expires, password);

        [HttpGet("{guestId:guid}")]
        public async Task<GuestDto> GetInfo(Guid guestId)
            => await guestBarrierService.GetGuest(guestId);

        [HttpPost("{guestId:guid}")]
        public async Task<GuestDto> OpenBarrier(Guid guestId)
            => await guestBarrierService.OpenBarrier(guestId);
    }
}