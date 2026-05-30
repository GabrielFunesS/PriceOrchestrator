using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceOrchestrator.Api.Entities;

namespace PriceOrchestrator.Api.Data.Configurations
{
    public class ProductCurrentPriceConfiguration
    : IEntityTypeConfiguration<ProductCurrentPrice>
    {
        public void Configure(EntityTypeBuilder<ProductCurrentPrice> builder)
        {
            builder.ToTable(nameof(ProductCurrentPrice));

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.ProductId)
                .IsUnique();

            builder.Property(x => x.ProductId)
                .IsRequired();

            builder.Property(x => x.BasePrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Currency)
                .HasMaxLength(3)
                .IsRequired();

            builder.Property(x => x.LastPriceChangeRequestId)
                .IsRequired();

            builder.Property(x => x.EffectiveFromUtc)
                .IsRequired();

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();

            builder.Property(x => x.UpdatedAtUtc)
                .IsRequired(false);

            builder.HasOne(x => x.Product)
                .WithOne(x => x.CurrentPrice)
                .HasForeignKey<ProductCurrentPrice>(x => x.ProductId);

            builder.HasOne(x => x.PriceChange)
                .WithOne(x => x.CurrentPrice)
                .HasForeignKey<ProductCurrentPrice>(x => x.LastPriceChangeRequestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
