using System.ComponentModel.DataAnnotations;
using BarrierLayer.Domain.Dto;

namespace BarrierLayer.Domain.Models
{
    public class Guest
    {
        [Key] public Guid Id { get; set; }
        public Barrier Barrier { get; set; }
        public DateTime Expires { get; set; }

        public static implicit operator GuestDto(Guest guest) => guest.ToDto();

        public GuestDto ToDto()
            => new GuestDto()
            {
                Id = Id,
                Expires = Expires,
                Address = Barrier.Address
            };
    }
}