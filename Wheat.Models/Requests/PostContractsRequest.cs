using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wheat.Models.Requests
{
    public class PostContractsRequest
    {
        [Required]
        public int Count { get; set; }
        [Required]
        public double Price { get; set; }
        public bool? Sell { get; set; }
    }
}
