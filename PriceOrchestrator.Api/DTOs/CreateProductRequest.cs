namespace PriceOrchestrator.Api.DTOs
{
    public class CreateProductRequest
    {
        public string ExternalId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
    }
}