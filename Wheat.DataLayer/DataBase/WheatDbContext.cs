using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wheat.Models.Entities;

namespace Wheat.DataLayer.DataBase
{
    public class WheatDbContext : IdentityDbContext<WIdentityUser>
    {
        public WheatDbContext(DbContextOptions<WheatDbContext> options):base(options){}
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<SellContract> SellContracts { get; set; }
        public virtual DbSet<OperationResult> OperationResults { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<WIdentityUser>().HasIndex(u=>u.Code).IsUnique();
            builder.Entity<SellContract>(e =>
            {
                e.HasOne(p => p.Seller)
                    .WithMany(m => m.SelfSellContracts)
                    .HasPrincipalKey(p => p.Code)
                    .HasForeignKey(f=>f.SellerCode);
                e.HasOne(p => p.Buyer)
                    .WithMany(m => m.SelfBuyContracts)
                    .HasPrincipalKey(p => p.Code)
                    .HasForeignKey(f=>f.PurchaserCode);
            });

            base.OnModelCreating(builder);
        }
    }
}
