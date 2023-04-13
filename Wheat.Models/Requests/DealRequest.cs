using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Wheat.Models.Requests
{
    public class DealRequest
    {
        [Required]
        [StringLength(5)]
        public string Code { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public double Price { get; set; }
        public bool? Buy { get; set; }
    }
}
