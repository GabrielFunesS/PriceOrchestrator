namespace PriceOrchestrator.Api.Entities
{
    public class Product : BaseEntity
    {
        public string ExternalId { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public string? Brand { get; set; }

        public string? Category { get; set; }

        public bool IsActive { get; set; }

        public string? InvalidationReason { get; set; }

        public ProductCurrentPrice? CurrentPrice { get; set; } = default!;
        public ICollection<PriceChangeRequest> PriceChangeRequests { get; set; } = new List<PriceChangeRequest>();
        public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
        public ICollection<ProductStock> Stocks { get; set; } = new List<ProductStock>();
    }
}
