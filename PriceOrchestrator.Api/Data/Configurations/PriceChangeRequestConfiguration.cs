using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceOrchestrator.Api.Entities;

namespace PriceOrchestrator.Api.Data.Configurations
{
    public class PriceChangeRequestConfiguration : IEntityTypeConfiguration<PriceChangeRequest>
    {
        public void Configure(EntityTypeBuilder<PriceChangeRequest> builder)
        {
            builder.ToTable(nameof(PriceChangeRequest));

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.ProductId);

            builder.HasIndex(x => new { x.Status, x.EffectiveFromUtc });

            builder.Property(x => x.ProductId)
                .IsRequired();

            builder.Property(x => x.OldPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.NewPrice)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.Currency)
                .HasMaxLength(3)
                .IsRequired();

            builder.Property(x => x.EffectiveFromUtc)
                .IsRequired();

            builder.Property(x => x.AppliedAtUtc)
                .IsRequired(false);

            builder.Property(x => x.Status);

            builder.Property(x => x.RequestedBy)
                .HasMaxLength(100);

            builder.Property(x => x.RequestSource)
                .HasMaxLength(50);

            builder.Property(x => x.RejectionReason)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();

            builder.Property(x => x.UpdatedAtUtc)
                .IsRequired(false);

            builder.HasOne(x => x.Product)                    
                .WithMany(x => x.PriceChangeRequests)  
                .HasForeignKey(x => x.ProductId)     
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
