using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarrierLayer.Dto
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
