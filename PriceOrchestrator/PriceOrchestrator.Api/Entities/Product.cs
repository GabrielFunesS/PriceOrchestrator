namespace PriceOrchestrator.Api.Entities
{
    public class Product
    {
        public Guid Id { get; set; }

        public string ExternalId { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public string? Brand { get; set; }

        public string? Category { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime? UpdatedAtUtc { get; set; }
    }
}
