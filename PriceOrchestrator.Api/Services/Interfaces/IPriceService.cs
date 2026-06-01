namespace PriceOrchestrator.Api.Services.Interfaces
{
    public interface IPriceService
    {
        Task<decimal> GetCurrentPriceAsync(Guid productId);
    }
}
