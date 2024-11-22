using System.ComponentModel.DataAnnotations;

namespace BarrierLayer.Domain.Models
{
    public class User
    {
        [Key] public int Id { get; set; }
        public string Number { get; set; }
        public Guid Token { get; set; }
        public List<UserBarrier> Barriers { get; set; }
        public UserStatus Status { get; set; }

        public User()
        {
            Barriers = new List<UserBarrier>();
        }
    }

    public enum UserStatus
    {
        Registering,
        Works,
        Blocked
    }
}