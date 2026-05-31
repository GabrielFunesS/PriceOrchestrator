using PriceOrchestrator.Api.DTOs;

namespace PriceOrchestrator.Api.Services.Interfaces
{
    public interface IStockService
    {
        Task<List<ProductStockDto>> GetStockByProductAsync(Guid productId);
        Task<bool> ReserveStockAsync(Guid productId, string warehouse, decimal quantity);
    }
}
