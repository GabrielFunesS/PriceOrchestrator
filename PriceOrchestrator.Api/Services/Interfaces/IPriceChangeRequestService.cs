using PriceOrchestrator.Api.DTOs;

namespace PriceOrchestrator.Api.Services.Interfaces
{
    public interface IPriceChangeRequestService
    {
        Task<Guid> CreateRequestAsync(CreatePriceChangeDto dto);
        Task<bool> CancelRequestAsync(Guid requestId);
    }
}
