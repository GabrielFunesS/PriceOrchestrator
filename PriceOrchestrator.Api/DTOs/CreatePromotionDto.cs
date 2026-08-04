using System.ComponentModel.DataAnnotations;
using PriceOrchestrator.Api.Entities.Enums;

namespace PriceOrchestrator.Api.DTOs
{
    public class CreatePromotionDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = default!;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Required]
        public PromotionType PromotionType { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Value { get; set; }

        [Required]
        public DateTime StartsAtUtc { get; set; }

        [Required]
        public DateTime EndsAtUtc { get; set; }

        public bool IsStackable { get; set; }

        public int Priority { get; set; }
    }
}
