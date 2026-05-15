using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceOrchestrator.Api.Entities;


namespace PriceOrchestrator.Api.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product));

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.ExternalId)
                .IsUnique();

            builder.HasIndex(x => new { x.Category, x.Brand });

            builder.Property(x => x.ExternalId)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(2000);


            builder.Property(x => x.Brand)
                .HasMaxLength(200);


            builder.Property(x => x.Category)
                .HasMaxLength(200);


            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();

            builder.Property(x => x.UpdatedAtUtc)
                .IsRequired(false);

        }
    }
}
