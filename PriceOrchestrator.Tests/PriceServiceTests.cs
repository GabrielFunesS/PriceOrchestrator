using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PriceOrchestrator.Api.Data.Context;
using PriceOrchestrator.Api.Entities;
using Xunit;

namespace PriceOrchestrator.Api.Services
{
    public class PriceServiceTests
    {
        private static AppDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetCurrentPriceAsync_ReturnsBasePrice_WhenExists()
        {
            var productId = Guid.NewGuid();
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            db.ProductCurrentPrices.Add(new ProductCurrentPrice
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                BasePrice = 123.45m,
                Currency = "USD",
                EffectiveFromUtc = DateTime.UtcNow,
                LastPriceChangeRequestId = Guid.NewGuid()
            });

            await db.SaveChangesAsync();

            var svc = new PriceService(db);

            var result = await svc.GetCurrentPriceAsync(productId);

            result.Should().Be(123.45m);
        }

        [Fact]
        public async Task GetCurrentPriceAsync_ReturnsZero_WhenNotFound()
        {
            var productId = Guid.NewGuid();
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            var svc = new PriceService(db);

            var result = await svc.GetCurrentPriceAsync(productId);

            result.Should().Be(0m);
        }
    }
}
