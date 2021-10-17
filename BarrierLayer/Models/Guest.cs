using System;
using System.ComponentModel.DataAnnotations;
using BarrierLayer.Dto;

namespace BarrierLayer.Models
{
    public class Guest
    {
        [Key]
        public Guid Id { get; set; }
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