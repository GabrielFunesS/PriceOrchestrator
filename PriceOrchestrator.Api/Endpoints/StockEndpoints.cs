using Microsoft.AspNetCore.Mvc;
using PriceOrchestrator.Api.DTOs;

namespace PriceOrchestrator.Api.Endpoints
{
    public static class StockEndpoints
    {
        public static void MapStockEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/stock").WithTags("Stock");

            group.MapGet("/product/{productId:guid}", async (Services.Interfaces.IStockService stockService, Guid productId) =>
            {
                var stocks = await stockService.GetStockByProductAsync(productId);
                return Results.Ok(stocks);
            });

            group.MapPost("/reserve", async (Services.Interfaces.IStockService stockService, ProductStockDto dto) =>
            {
                var reserved = await stockService.ReserveStockAsync(dto.ProductId, dto.Warehouse, dto.Quantity);
                return reserved ? Results.Ok() : Results.BadRequest();
            });
        }
    }
}
