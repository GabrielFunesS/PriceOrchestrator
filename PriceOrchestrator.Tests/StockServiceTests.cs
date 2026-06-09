using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PriceOrchestrator.Api.Data.Context;
using PriceOrchestrator.Api.Entities;
using Xunit;

namespace PriceOrchestrator.Api.Services
{
    public class StockServiceTests
    {
        private static AppDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetStockByProductAsync_ReturnsStocks()
        {
            var productId = Guid.NewGuid();
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            db.ProductStocks.AddRange(new ProductStock
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Quantity = 10m,
                ReservedQuantity = 1m,
                Warehouse = "W1"
            }, new ProductStock
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Quantity = 5m,
                ReservedQuantity = 0m,
                Warehouse = "W2"
            });

            await db.SaveChangesAsync();

            var svc = new StockService(db);

            var list = await svc.GetStockByProductAsync(productId);

            list.Should().HaveCount(2);
            list.Select(s => s.Warehouse).Should().Contain(new[] { "W1", "W2" });
        }

        [Fact]
        public async Task ReserveStockAsync_ReturnsFalse_WhenInsufficient()
        {
            var productId = Guid.NewGuid();
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            db.ProductStocks.Add(new ProductStock
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Quantity = 2m,
                ReservedQuantity = 1.5m,
                Warehouse = "W"
            });

            await db.SaveChangesAsync();

            var svc = new StockService(db);

            var res = await svc.ReserveStockAsync(productId, "W", 1m);

            res.Should().BeFalse();
        }

        [Fact]
        public async Task ReserveStockAsync_Reserves_WhenSufficient()
        {
            var productId = Guid.NewGuid();
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            var stock = new ProductStock
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Quantity = 10m,
                ReservedQuantity = 2m,
                Warehouse = "W"
            };

            db.ProductStocks.Add(stock);
            await db.SaveChangesAsync();

            var svc = new StockService(db);

            var res = await svc.ReserveStockAsync(productId, "W", 3m);

            res.Should().BeTrue();
            var inDb = await db.ProductStocks.FindAsync(stock.Id);
            inDb!.ReservedQuantity.Should().Be(5m);
        }
    }
}
