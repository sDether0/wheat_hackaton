
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;

namespace Wheat.Models.Entities
{
    public class WIdentityUser : IdentityUser
    {
        public string Code { get; set; }

        public virtual ICollection<SellContract> SelfSellContracts { get; set; }
        public virtual ICollection<SellContract> SelfBuyContracts { get; set; }
    }
}
