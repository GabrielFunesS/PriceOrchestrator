using Microsoft.EntityFrameworkCore;
using PriceOrchestrator.Api.Data.Context;
using PriceOrchestrator.Api.DTOs;
using PriceOrchestrator.Api.Services.Interfaces;
using PriceOrchestrator.Api.Entities.Enums;

namespace PriceOrchestrator.Api.Services
{
    public class PriceChangeRequestService : IPriceChangeRequestService
    {
        private readonly AppDbContext _db;

        public PriceChangeRequestService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> CreateRequestAsync(CreatePriceChangeDto dto)
        {
            var request = new Entities.PriceChangeRequest
            {
                Id = Guid.NewGuid(),
                ProductId = dto.ProductId,
                OldPrice = dto.OldPrice,
                NewPrice = dto.NewPrice,
                Currency = dto.Currency,
                EffectiveFromUtc = dto.EffectiveFromUtc,
                Status = PriceChangeRequestStatus.Pending,
                RequestedBy = dto.RequestedBy,
                RequestSource = dto.RequestSource,
                CreatedAtUtc = DateTime.UtcNow
            };

            _db.PriceChangeRequests.Add(request);
            await _db.SaveChangesAsync();
            return request.Id;
        }

        public async Task<bool> CancelRequestAsync(Guid requestId)
        {
            var req = await _db.PriceChangeRequests.FindAsync(requestId);
            if (req is null)
                return false;

            req.Status = PriceChangeRequestStatus.Cancelled;
            req.UpdatedAtUtc = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
