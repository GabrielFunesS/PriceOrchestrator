using PriceOrchestrator.Api.DTOs;

namespace PriceOrchestrator.Api.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<Guid> CreatePromotionAsync(CreatePromotionDto dto);
        Task<List<PromotionDto>> GetActivePromotionsByProductAsync(Guid productId);
    }
}
