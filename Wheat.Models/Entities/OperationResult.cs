using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wheat.Models.Entities
{
    public class OperationResult
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string SellerId { get; set; }
        public string? BuyerId { get; set; }
        public DateTime Time { get; set; }
        public double Balance { get; set; }
        public bool Complete { get; set; }
        public double OldPrice { get; set; }
        public double NewPrice { get; set; }
        public int Count { get; set; }

        public virtual WIdentityUser User { get; set; }
        public virtual WIdentityUser Seller { get; set; }
        public virtual WIdentityUser? Buyer { get; set; }
    }
}
