using Microsoft.EntityFrameworkCore;
using PriceOrchestrator.Api.Data.Context;
using PriceOrchestrator.Api.Services.Interfaces;

namespace PriceOrchestrator.Api.Services
{
    public class PriceService : IPriceService
    {
        private readonly AppDbContext _db;

        public PriceService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<decimal> GetCurrentPriceAsync(Guid productId)
        {
            var currentPrice = await _db.ProductCurrentPrices
                .Where(p => p.ProductId == productId)
                .Select(p => p.BasePrice)
                .FirstOrDefaultAsync();

            return currentPrice;
        }
    }
}
