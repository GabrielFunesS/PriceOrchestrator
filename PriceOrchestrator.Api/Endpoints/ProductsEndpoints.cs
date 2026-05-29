using Microsoft.AspNetCore.Mvc;
using PriceOrchestrator.Api.DTOs;
using PriceOrchestrator.Api.Services;

namespace PriceOrchestrator.Api.Endpoints
{
    public static class ProductsEndpoints
    {
        public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/products").WithTags("Products");

            group.MapGet("/", async (ProductService service, [FromQuery] int page = 1, [FromQuery] int pageSize = 20) =>
            {
                var products = await service.GetAllAsync(page, pageSize);
                return Results.Ok(products);
            });

            group.MapGet("/{id:guid}", async (ProductService service, Guid id) =>
            {
                var product = await service.GetByIdAsync(id);
                return product is null ? Results.NotFound() : Results.Ok(product);
            });

            group.MapPost("/", async (ProductService service, CreateProductRequest request) =>
            {
                var product = await service.CreateAsync(request);
                return Results.Created($"/api/products/{product.Id}", product);
            });

            group.MapPut("/{id:guid}", async (ProductService service, Guid id, UpdateProductRequest request) =>
            {
                var updated = await service.UpdateAsync(id, request);
                return updated ? Results.NoContent() : Results.NotFound();
            });

            group.MapDelete("/{id:guid}", async (ProductService service, Guid id) =>
            {
                var deleted = await service.DeleteAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}