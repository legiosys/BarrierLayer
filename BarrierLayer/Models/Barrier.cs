using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarrierLayer.Models
{
    public class Barrier
    {
        [Key]
        public int Id { get; set; }
        public string BarrierNumber { get; set; }
        public string Token { get; set; }
        public string UserNumber { get; set; }
        public BarrierType BarrierType { get; set; }
        public List<UserBarrier> UserBarriers { get; set; }

        public Barrier()
        {
            UserBarriers = new List<UserBarrier>();
        }
    }
}
