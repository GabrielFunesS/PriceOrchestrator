namespace PriceOrchestrator.Api.Entities
{
    public class Product : BaseEntity
    {
        public string ExternalId { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public string? Brand { get; set; }

        public string? Category { get; set; }

        public ProductCurrentPrice CurrentPrice { get; set; } = default!;
        public ICollection<PriceChangeRequest> PriceChangeRequests { get; set; } = [];
        public ICollection<Promotion> Promotions { get; set; } = [];
        public ICollection<ProductStock> Stocks { get; set; } = [];
    }
}
