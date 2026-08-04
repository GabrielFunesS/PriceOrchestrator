using PriceOrchestrator.Api.Entities.Enums;

namespace PriceOrchestrator.Api.DTOs
{
    public class CreatePromotionDto
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; } = default!;

        public string? Description { get; set; }

        public PromotionType PromotionType { get; set; }

        public decimal Value { get; set; }

        public DateTime StartsAtUtc { get; set; }

        public DateTime EndsAtUtc { get; set; }

        public bool IsStackable { get; set; }

        public int Priority { get; set; }
    }
}
