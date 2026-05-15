using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceOrchestrator.Api.Entities;

namespace PriceOrchestrator.Api.Data.Configurations
{
    public class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
    {
        public void Configure(EntityTypeBuilder<ProductStock> builder)
        {
            builder.ToTable(nameof(ProductStock));

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.ProductId);

            builder.HasIndex(x => new { x.ProductId, x.Warehouse })
                .IsUnique();

            builder.Property(x => x.ProductId)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .HasPrecision(18, 4)
                .IsRequired();

            builder.Property(x => x.ReservedQuantity)
                .HasPrecision(18, 4)
                .IsRequired();

            builder.Property(x => x.Warehouse)
               .HasMaxLength(100)
               .IsRequired();

            builder.Property(x => x.LastMovementAtUtc)
                .IsRequired(false);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Stocks)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
