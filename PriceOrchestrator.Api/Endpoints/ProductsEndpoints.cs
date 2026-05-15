namespace PriceOrchestrator.Api.Endpoints
{
    public static class ProductsEndpoints
    {
        public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/products", async () =>
            {
            });

            app.MapGet("/api/products/{id:guid}", async (Guid id) =>
            {
            });
        }
    }
}
