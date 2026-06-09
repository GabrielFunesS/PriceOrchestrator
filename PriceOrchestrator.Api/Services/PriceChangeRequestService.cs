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

        public async Task ProcessPendingAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            var today = now.Date; // Capturamos la fecha actual (00:00:00) para la comparación de días

            // 1. Traemos todas las solicitudes pendientes cuyo momento de activación ya llegó o pasó
            var requests = await _db.PriceChangeRequests
                .Where(r => r.Status == PriceChangeRequestStatus.Pending && r.EffectiveFromUtc <= now)
                .Include(r => r.Product)
                    .ThenInclude(p => p.CurrentPrice)
                .ToListAsync(cancellationToken);

            if (requests.Count == 0)
                return;

            foreach (var req in requests)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // 2. REGLA DE NEGOCIO: Si la fecha planificada es de un día anterior a hoy, la ventana se cerró
                if (req.EffectiveFromUtc.Date < today)
                {
                    req.Status = PriceChangeRequestStatus.Expired;
                    req.UpdatedAtUtc = now;
                    // Usamos continue para saltar a la siguiente solicitud sin alterar el precio maestro del producto
                    continue;
                }

                // 3. Si la solicitud es del día de hoy, procedemos a aplicar el cambio normalmente
                if (req.Product is null)
                    continue;

                var current = req.Product.CurrentPrice;

                if (current is null)
                {
                    // Producto nuevo: creamos su primer registro de precio actual
                    current = new Entities.ProductCurrentPrice
                    {
                        Id = Guid.NewGuid(),
                        ProductId = req.ProductId,
                        BasePrice = req.NewPrice,
                        Currency = req.Currency,
                        EffectiveFromUtc = req.EffectiveFromUtc,
                        LastPriceChangeRequestId = req.Id,
                    };
                    _db.ProductCurrentPrices.Add(current);
                }
                else
                {
                    // Producto existente: actualizamos el precio vigente
                    current.BasePrice = req.NewPrice;
                    current.Currency = req.Currency;
                    current.EffectiveFromUtc = req.EffectiveFromUtc;
                    current.LastPriceChangeRequestId = req.Id;
                    current.UpdatedAtUtc = now;
                }

                // Marcar la solicitud como exitosa
                req.Status = PriceChangeRequestStatus.Applied;
                req.AppliedAtUtc = now;
                req.UpdatedAtUtc = now;
            }

            // 4. Impactamos todo en la base de datos en un único viaje transaccional
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
