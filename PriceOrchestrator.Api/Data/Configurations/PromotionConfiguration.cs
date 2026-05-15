using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceOrchestrator.Api.Entities;

namespace PriceOrchestrator.Api.Data.Configurations
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.ToTable(nameof(Promotion));

            builder.HasKey(p => p.Id);

            builder.HasIndex(x => x.ProductId);

            builder.Property(x => x.ProductId)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(x => x.PromotionType)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.Value)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.StartsAtUtc)
                .IsRequired();

            builder.Property(x => x.EndsAtUtc)
                .IsRequired();

            builder.Property(x => x.IsStackable)
                .IsRequired();

            builder.Property(x => x.Priority)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();

            builder.Property(x => x.UpdatedAtUtc)
                .IsRequired(false);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Promotions)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
