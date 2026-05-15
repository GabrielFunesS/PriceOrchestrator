using Microsoft.EntityFrameworkCore;
using PriceOrchestrator.Api.Data.Context;
using PriceOrchestrator.Api.DTOs;
using PriceOrchestrator.Api.Entities;

namespace PriceOrchestrator.Api.Services
{
    public class ProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ProductDto>> GetAllAsync(int page = 1, int pageSize = 20)
        {
            return await _db.Products
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    ExternalId = p.ExternalId,
                    Name = p.Name,
                    Description = p.Description,
                    Brand = p.Brand,
                    Category = p.Category,
                    CreatedAtUtc = p.CreatedAtUtc
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id)
        {
            return await _db.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    ExternalId = p.ExternalId,
                    Name = p.Name,
                    Description = p.Description,
                    Brand = p.Brand,
                    Category = p.Category,
                    CreatedAtUtc = p.CreatedAtUtc
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ProductDto> CreateAsync(CreateProductRequest request)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                ExternalId = request.ExternalId,
                Name = request.Name,
                Description = request.Description,
                Brand = request.Brand,
                Category = request.Category,
                CreatedAtUtc = DateTime.UtcNow
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                ExternalId = product.ExternalId,
                Name = product.Name,
                Description = product.Description,
                Brand = product.Brand,
                Category = product.Category,
                CreatedAtUtc = product.CreatedAtUtc
            };
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateProductRequest request)
        {
            var product = await _db.Products.FindAsync(id);
            if (product is null)
                return false;

            product.Name = request.Name;
            product.Description = request.Description;
            product.Brand = request.Brand;
            product.Category = request.Category;
            product.UpdatedAtUtc = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product is null)
                return false;

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}