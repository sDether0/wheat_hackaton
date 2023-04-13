using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wheat.Models.Requests
{
    public class ReSellRequest
    {
        [Required]
        public double OldPrice { get; set; }
        [Required]
        public double NewPrice { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
