using System.ComponentModel.DataAnnotations;

namespace BarrierLayer.Domain.Models
{
    public class Configuration
    {
        [Key] public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Format { get; set; }
    }
}