using Microsoft.EntityFrameworkCore;
using System.Linq;
using PriceOrchestrator.Api.Data.Context;
using PriceOrchestrator.Api.DTOs;
using PriceOrchestrator.Api.Services.Interfaces;

namespace PriceOrchestrator.Api.Services
{
    public class StockService : IStockService
    {
        private readonly AppDbContext _db;

        public StockService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ProductStockDto>> GetStockByProductAsync(Guid productId)
        {
            return await _db.ProductStocks
                .Where(s => s.ProductId == productId)
                .Select(s => new ProductStockDto
                {
                    ProductId = s.ProductId,
                    Quantity = s.Quantity,
                    ReservedQuantity = s.ReservedQuantity,
                    Warehouse = s.Warehouse
                })
                .ToListAsync();
        }

        public async Task<bool> ReserveStockAsync(Guid productId, string warehouse, decimal quantity)
        {
            var stock = await _db.ProductStocks
                .Where(s => s.ProductId == productId && s.Warehouse == warehouse)
                .FirstOrDefaultAsync();

            if (stock is null)
                return false;

            if (stock.Quantity - stock.ReservedQuantity < quantity)
                return false;

            stock.ReservedQuantity += quantity;
            stock.LastMovementAtUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
