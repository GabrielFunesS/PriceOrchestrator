using Microsoft.EntityFrameworkCore;
using PriceOrchestrator.Api.Data.Context;
using PriceOrchestrator.Api.DTOs;
using PriceOrchestrator.Api.Entities;
using PriceOrchestrator.Api.Entities.Enums;
using PriceOrchestrator.Api.Services.Interfaces;

namespace PriceOrchestrator.Api.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly AppDbContext _db;

        public PromotionService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> CreatePromotionAsync(CreatePromotionDto dto)
        {
            var promotion = new Promotion
            {
                Id = Guid.NewGuid(),
                ProductId = dto.ProductId,
                Name = dto.Name,
                Description = dto.Description,
                PromotionType = dto.PromotionType,
                Value = dto.Value,
                StartsAtUtc = dto.StartsAtUtc,
                EndsAtUtc = dto.EndsAtUtc,
                IsStackable = dto.IsStackable,
                Priority = dto.Priority,
                CreatedAtUtc = DateTime.UtcNow,
                Status = PromotionStatus.Active
            };

            _db.Promotions.Add(promotion);
            await _db.SaveChangesAsync();
            return promotion.Id;
        }

        public async Task<List<PromotionDto>> GetActivePromotionsByProductAsync(Guid productId)
        {
            return await _db.Promotions
                .Where(p => p.ProductId == productId && p.Status == PromotionStatus.Active)
                .Select(p => new PromotionDto
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    PromotionType = p.PromotionType,
                    Value = p.Value,
                    StartsAtUtc = p.StartsAtUtc,
                    EndsAtUtc = p.EndsAtUtc,
                    IsStackable = p.IsStackable,
                    Priority = p.Priority,
                    Status = p.Status.ToString()
                })
                .ToListAsync();
        }
    }
}
