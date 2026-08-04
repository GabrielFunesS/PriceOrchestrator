using System.ComponentModel.DataAnnotations;

namespace PriceOrchestrator.Api.DTOs
{
    public class ProductStockDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Range(0.0001, double.MaxValue)]
        public decimal Quantity { get; set; }

        public decimal ReservedQuantity { get; set; }

        [Required]
        [StringLength(200)]
        public string Warehouse { get; set; } = default!;
    }
}
