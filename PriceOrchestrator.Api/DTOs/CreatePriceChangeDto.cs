namespace PriceOrchestrator.Api.DTOs
{
    public class CreatePriceChangeDto
    {
        public Guid ProductId { get; set; }

        public decimal OldPrice { get; set; }

        public decimal NewPrice { get; set; }

        public string Currency { get; set; } = default!;

        public DateTime EffectiveFromUtc { get; set; }

        public string RequestedBy { get; set; } = default!;

        public string RequestSource { get; set; } = string.Empty;
    }
}
