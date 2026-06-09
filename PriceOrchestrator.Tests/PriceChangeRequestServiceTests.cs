using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PriceOrchestrator.Api.Data.Context;
using PriceOrchestrator.Api.Entities;
using PriceOrchestrator.Api.Entities.Enums;
using PriceOrchestrator.Api.Services;
using Xunit;

namespace PriceOrchestrator.Api.Services
{
    public class PriceChangeRequestServiceTests
    {
        private static AppDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateRequestAsync_PersistsRequest()
        {
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            var svc = new PriceChangeRequestService(db);

            var dto = new PriceOrchestrator.Api.DTOs.CreatePriceChangeDto
            {
                ProductId = Guid.NewGuid(),
                OldPrice = 10m,
                NewPrice = 12m,
                Currency = "USD",
                EffectiveFromUtc = DateTime.UtcNow.AddHours(1),
                RequestedBy = "user",
                RequestSource = "api"
            };

            var id = await svc.CreateRequestAsync(dto);

            id.Should().NotBeEmpty();

            var inDb = await db.PriceChangeRequests.FindAsync(id);
            inDb.Should().NotBeNull();
            inDb!.Status.Should().Be(PriceChangeRequestStatus.Pending);
        }

        [Fact]
        public async Task CancelRequestAsync_ReturnsFalse_WhenNotFound()
        {
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            var svc = new PriceChangeRequestService(db);

            var res = await svc.CancelRequestAsync(Guid.NewGuid());

            res.Should().BeFalse();
        }

        [Fact]
        public async Task ProcessPendingAsync_AppliesRequest_ForToday()
        {
            var productId = Guid.NewGuid();
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            var product = new Product { Id = productId, ExternalId = "e", Name = "N", CreatedAtUtc = DateTime.UtcNow };
            db.Products.Add(product);

            var req = new PriceChangeRequest
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                OldPrice = 10m,
                NewPrice = 15m,
                Currency = "USD",
                EffectiveFromUtc = DateTime.UtcNow,
                Status = PriceChangeRequestStatus.Pending,
                RequestedBy = "u",
                RequestSource = "s",
                CreatedAtUtc = DateTime.UtcNow
            };

            db.PriceChangeRequests.Add(req);
            await db.SaveChangesAsync();

            var svc = new PriceChangeRequestService(db);

            await svc.ProcessPendingAsync(CancellationToken.None);

            var updated = await db.PriceChangeRequests.FindAsync(req.Id);
            updated!.Status.Should().Be(PriceChangeRequestStatus.Applied);

            var current = db.ProductCurrentPrices.FirstOrDefault(p => p.ProductId == productId);
            current.Should().NotBeNull();
            current!.BasePrice.Should().Be(15m);
        }

        [Fact]
        public async Task ProcessPendingAsync_ExpiresPastRequests()
        {
            var productId = Guid.NewGuid();
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            var product = new Product { Id = productId, ExternalId = "e", Name = "N", CreatedAtUtc = DateTime.UtcNow };
            db.Products.Add(product);

            var req = new PriceChangeRequest
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                OldPrice = 10m,
                NewPrice = 15m,
                Currency = "USD",
                EffectiveFromUtc = DateTime.UtcNow.AddDays(-2),
                Status = PriceChangeRequestStatus.Pending,
                RequestedBy = "u",
                RequestSource = "s",
                CreatedAtUtc = DateTime.UtcNow.AddDays(-3)
            };

            db.PriceChangeRequests.Add(req);
            await db.SaveChangesAsync();

            var svc = new PriceChangeRequestService(db);

            await svc.ProcessPendingAsync(CancellationToken.None);

            var updated = await db.PriceChangeRequests.FindAsync(req.Id);
            updated!.Status.Should().Be(PriceChangeRequestStatus.Expired);
        }
    }
}
