namespace PriceOrchestrator.Api.DTOs
{
    public class CurrentPriceDto
    {
        public Guid ProductId { get; set; }
        public decimal BasePrice { get; set; }
        public decimal EffectivePrice { get; set; }
        public List<string> AppliedPromotionsNames { get; set; }
    }
}
