using System.ComponentModel.DataAnnotations;

namespace BarrierLayer.Domain.Models
{
    public class UserBarrier
    {
        [Key] public int UserId { get; set; }
        [Key] public int BarrierId { get; set; }
    }
}