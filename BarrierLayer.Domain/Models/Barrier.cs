using System.ComponentModel.DataAnnotations;

namespace BarrierLayer.Domain.Models
{
    public class Barrier
    {
        [Key] public int Id { get; set; }
        public string BarrierNumber { get; set; }
        public string? Token { get; set; }
        public string UserNumber { get; set; }
        public string? Address { get; set; }
        public BarrierType BarrierType { get; set; }
        public List<UserBarrier> UserBarriers { get; set; } = new();
    }
}