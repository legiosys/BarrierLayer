using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarrierLayer.Models
{
    public class UserBarrier
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int BarrierId { get; set; }
    }
}
