using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wheat.Models.Requests
{
    public class ReDealRequest
    {
        [Required]
        public string BuyerCode { get; set; }
        [Required]
        public string SellerCode { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Count { get; set; }
    }
}
