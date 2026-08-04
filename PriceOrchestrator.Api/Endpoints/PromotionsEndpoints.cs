using Microsoft.AspNetCore.Mvc;
using PriceOrchestrator.Api.DTOs;
using PriceOrchestrator.Api.Extensions;

namespace PriceOrchestrator.Api.Endpoints
{
    public static class PromotionsEndpoints
    {
        public static void MapPromotionsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/promotions").WithTags("Promotions");

            group.MapGet("/product/{productId:guid}", async (Services.Interfaces.IPromotionService promotionService, Guid productId) =>
            {
                var promotions = await promotionService.GetActivePromotionsByProductAsync(productId);
                return Results.Ok(promotions);
            });

            group.MapPost("/", async (Services.Interfaces.IPromotionService promotionService, CreatePromotionDto dto) =>
            {
                var id = await promotionService.CreatePromotionAsync(dto);
                return Results.Created($"/api/promotions/{id}", id);
            });
        }
    }
}
