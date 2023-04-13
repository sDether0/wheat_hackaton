using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wheat.Models.Entities
{
    public class SellContract
    {
        [JsonIgnore]
        public string Id { get; set; }
        public string? SellerCode { get; set; }
        public string? PurchaserCode { get; set; }
        public double Price { get; set; }
        public DateTime? Updated { get; set; }
        public bool OnTrade { get; set; }
        [JsonIgnore]
        public double OldPrice { get; set; }

        [JsonIgnore]
        public virtual WIdentityUser? Seller { get; set; }
        
        [JsonIgnore]
        public virtual WIdentityUser? Buyer { get; set; }
    }
}
