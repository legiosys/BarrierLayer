namespace BarrierLayer.Domain.Dto
{
    public class BarrierAddResult
    {
        public int Id { get; set; }
        public BarrierAddStatus Status { get; set; }
    }

    public enum BarrierAddStatus
    {
        Confirmed,
        WaitForConfirmation,
        Error
    }
}