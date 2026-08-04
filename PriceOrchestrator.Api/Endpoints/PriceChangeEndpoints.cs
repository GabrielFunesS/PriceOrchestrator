using Microsoft.AspNetCore.Mvc;
using PriceOrchestrator.Api.DTOs;
using PriceOrchestrator.Api.Extensions;

namespace PriceOrchestrator.Api.Endpoints
{
    public static class PriceChangeEndpoints
    {
        public static void MapPriceChangeEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/price-changes").WithTags("PriceChanges");

            group.MapPost("/", async (Services.Interfaces.IPriceChangeRequestService requestService, CreatePriceChangeDto dto) =>
            {
                var id = await requestService.CreateRequestAsync(dto);
                return Results.Created($"/api/price-changes/{id}", id);
            });

            group.MapPost("/{requestId:guid}/cancel", async (Services.Interfaces.IPriceChangeRequestService requestService, Guid requestId) =>
            {
                var canceled = await requestService.CancelRequestAsync(requestId);
                return canceled ? Results.Ok() : Results.NotFound();
            });
        }
    }
}
