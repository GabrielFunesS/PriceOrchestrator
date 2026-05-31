namespace PriceOrchestrator.Api.DTOs
{
    public class CreatePriceChangeDto
    {
        public Guid ProductId { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public string Currency { get; set; }
        public DateTime EffectiveFromUtc { get; set; }
        public string RequestedBy { get; set; }
        public string RequestSource { get; set; } = string.Empty;
    }
}
