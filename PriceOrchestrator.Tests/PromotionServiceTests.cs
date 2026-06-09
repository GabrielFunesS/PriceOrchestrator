using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PriceOrchestrator.Api.Data.Context;
using PriceOrchestrator.Api.DTOs;
using PriceOrchestrator.Api.Entities;
using PriceOrchestrator.Api.Entities.Enums;
using Xunit;

namespace PriceOrchestrator.Api.Services
{
    public class PromotionServiceTests
    {
        private static AppDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreatePromotionAsync_PersistsPromotionAndReturnsId()
        {
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            var svc = new PromotionService(db);

            var dto = new CreatePromotionDto
            {
                ProductId = Guid.NewGuid(),
                Name = "Promo 1",
                Description = "D",
                PromotionType = PromotionType.Percentage,
                Value = 10m,
                StartsAtUtc = DateTime.UtcNow,
                EndsAtUtc = DateTime.UtcNow.AddDays(1),
                IsStackable = false,
                Priority = 1
            };

            var id = await svc.CreatePromotionAsync(dto);

            id.Should().NotBeEmpty();

            var inDb = await db.Promotions.FindAsync(id);
            inDb.Should().NotBeNull();
            inDb!.Name.Should().Be("Promo 1");
            inDb.Status.Should().Be(PromotionStatus.Active);
        }

        [Fact]
        public async Task GetActivePromotionsByProductAsync_ReturnsActivePromotions()
        {
            var productId = Guid.NewGuid();
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            db.Promotions.AddRange(new Promotion
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Name = "Active",
                PromotionType = PromotionType.FixedAmount,
                Value = 1m,
                StartsAtUtc = DateTime.UtcNow,
                EndsAtUtc = DateTime.UtcNow.AddDays(1),
                IsStackable = false,
                Priority = 1,
                Status = PromotionStatus.Active
            }, new Promotion
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Name = "Inactive",
                PromotionType = PromotionType.Percentage,
                Value = 2m,
                StartsAtUtc = DateTime.UtcNow,
                EndsAtUtc = DateTime.UtcNow.AddDays(1),
                IsStackable = false,
                Priority = 2,
                Status = PromotionStatus.Scheduled
            });

            await db.SaveChangesAsync();

            var svc = new PromotionService(db);

            var list = await svc.GetActivePromotionsByProductAsync(productId);

            list.Should().HaveCount(1);
            list[0].Name.Should().Be("Active");
        }
    }
}
