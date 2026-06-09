using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PriceOrchestrator.Api.Data.Context;
using PriceOrchestrator.Api.DTOs;
using PriceOrchestrator.Api.Entities;
using Xunit;

namespace PriceOrchestrator.Api.Services
{
    public class ProductServiceTests
    {
        private static AppDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsPagedOrderedProducts()
        {
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), ExternalId = "b", Name = "Banana", CreatedAtUtc = DateTime.UtcNow },
                new Product { Id = Guid.NewGuid(), ExternalId = "a", Name = "Apple", CreatedAtUtc = DateTime.UtcNow },
                new Product { Id = Guid.NewGuid(), ExternalId = "c", Name = "Carrot", CreatedAtUtc = DateTime.UtcNow }
            };

            db.Products.AddRange(products);
            await db.SaveChangesAsync();

            var svc = new ProductService(db);

            var result = await svc.GetAllAsync(page: 1, pageSize: 2);

            result.Should().HaveCount(2);
            result[0].Name.Should().Be("Apple");
            result[1].Name.Should().Be("Banana");
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProductDto_WhenExists()
        {
            var id = Guid.NewGuid();
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            db.Products.Add(new Product
            {
                Id = id,
                ExternalId = "ext-1",
                Name = "Prod",
                CreatedAtUtc = DateTime.UtcNow
            });

            await db.SaveChangesAsync();

            var svc = new ProductService(db);

            var dto = await svc.GetByIdAsync(id);

            dto.Should().NotBeNull();
            dto!.Id.Should().Be(id);
            dto.ExternalId.Should().Be("ext-1");
        }

        [Fact]
        public async Task CreateAsync_CreatesAndReturnsDto()
        {
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            var svc = new ProductService(db);

            var request = new CreateProductRequest
            {
                ExternalId = "ext-2",
                Name = "New Prod",
                Description = "Desc",
                Brand = "Brand",
                Category = "Cat"
            };

            var dto = await svc.CreateAsync(request);

            dto.Should().NotBeNull();
            dto.ExternalId.Should().Be("ext-2");

            var inDb = await db.Products.FindAsync(dto.Id);
            inDb.Should().NotBeNull();
            inDb!.Name.Should().Be("New Prod");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenNotFound()
        {
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            var svc = new ProductService(db);

            var res = await svc.UpdateAsync(Guid.NewGuid(), new DTOs.UpdateProductRequest { Name = "X", Brand = "B", Category = "C", Description = "D" });

            res.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_RemovesProduct_WhenExists()
        {
            var id = Guid.NewGuid();
            using var db = CreateInMemoryContext(Guid.NewGuid().ToString());

            db.Products.Add(new Product { Id = id, ExternalId = "e", Name = "N", CreatedAtUtc = DateTime.UtcNow });
            await db.SaveChangesAsync();

            var svc = new ProductService(db);

            var res = await svc.DeleteAsync(id);

            res.Should().BeTrue();
            var inDb = await db.Products.FindAsync(id);
            inDb.Should().BeNull();
        }
    }
}
