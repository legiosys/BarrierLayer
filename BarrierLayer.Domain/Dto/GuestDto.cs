namespace BarrierLayer.Domain.Dto
{
    public class GuestDto
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public DateTime Expires { get; set; }
    }
}