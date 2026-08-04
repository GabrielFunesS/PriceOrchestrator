using System.ComponentModel.DataAnnotations;

namespace PriceOrchestrator.Api.DTOs
{
    public class CreatePriceChangeDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal OldPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal NewPrice { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; } = default!;

        [Required]
        public DateTime EffectiveFromUtc { get; set; }

        [Required]
        [StringLength(100)]
        public string RequestedBy { get; set; } = default!;

        [StringLength(50)]
        public string RequestSource { get; set; } = string.Empty;
    }
}
