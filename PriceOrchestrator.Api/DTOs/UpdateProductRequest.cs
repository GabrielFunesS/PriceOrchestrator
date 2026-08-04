using System.ComponentModel.DataAnnotations;

namespace PriceOrchestrator.Api.DTOs
{
    public class UpdateProductRequest
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = default!;

        [StringLength(2000)]
        public string? Description { get; set; }

        [StringLength(200)]
        public string? Brand { get; set; }

        [StringLength(200)]
        public string? Category { get; set; }
    }
}