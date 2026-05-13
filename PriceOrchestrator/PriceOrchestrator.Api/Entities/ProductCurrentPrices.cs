namespace PriceOrchestrator.Api.Entities
{
    public class ProductCurrentPrice : BaseEntity
    {

        public Guid ProductId { get; set; }

        public decimal BasePrice { get; set; }

        public string Currency { get; set; } = default!;

        public DateTime EffectiveFromUtc { get; set; }

        public Guid? LastPriceChangeRequestId { get; set; }


        // Navigation Properties
        public Product Product { get; set; } = default!;
    }
}
