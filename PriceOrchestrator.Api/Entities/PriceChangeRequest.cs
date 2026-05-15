namespace PriceOrchestrator.Api.Entities
{
    public class PriceChangeRequest : BaseEntity
    {
        public Guid ProductId { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public string Currency { get; set; } = default!;
        public DateTime EffectiveFromUtc { get; set; }
        public DateTime? AppliedAtUtc { get; set; }
        public string Status { get; set; } = default!;

        public string RequestedBy { get; set; } = default!;
        public string RequestSource { get; set; } = default!;

        public string RejectionReason { get; set; } = default!;


        public Product Product { get; set; } = default!;
    }
}
