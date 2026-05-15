namespace PriceOrchestrator.Api.DTOs
{
    public class UpdateProductRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }
    }
}