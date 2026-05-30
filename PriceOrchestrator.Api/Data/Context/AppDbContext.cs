using Microsoft.EntityFrameworkCore;
using PriceOrchestrator.Api.Entities;

namespace PriceOrchestrator.Api.Data.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductCurrentPrice> ProductCurrentPrices => Set<ProductCurrentPrice>();
        public DbSet<PriceChangeRequest> PriceChangeRequests => Set<PriceChangeRequest>();
        public DbSet<Promotion> Promotions => Set<Promotion>();
        public DbSet<ProductStock> ProductStocks => Set<ProductStock>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
