namespace PriceOrchestrator.Api.DTOs
{
    public class ProductStockDto
    {
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }

        public decimal ReservedQuantity { get; set; }

        public string Warehouse { get; set; } = default!;
    }
}
