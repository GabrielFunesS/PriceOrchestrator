namespace PriceOrchestrator.Api.Entities
{
    public class ProductStock : BaseEntity
    {
        public Guid ProductId { get; set; }

        public decimal Quantity { get; set; }

        public decimal ReservedQuantity { get; set; }

        public string Warehouse { get; set; } = default!;

        public DateTime? LastMovementAtUtc { get; set; }

        // Navegación
        public Product Product { get; set; } = default!;
    }
}
