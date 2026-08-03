using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PriceOrchestrator.Api.Data.Context;
using PriceOrchestrator.Api.Entities.Enums;

namespace PriceOrchestrator.Api.Hosted
{
    public class PriceChangeProcessorWorker : BackgroundService
    {
        private readonly ILogger<PriceChangeProcessorWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public PriceChangeProcessorWorker(ILogger<PriceChangeProcessorWorker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PriceChangeProcessorWorker starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    // Use the domain service to process pending requests (reuses existing business logic)
                    var processor = scope.ServiceProvider.GetRequiredService<Services.Interfaces.IPriceChangeRequestService>();

                    await processor.ProcessPendingAsync(stoppingToken);

                    // Log remaining pending count for observability
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var pendingCount = await db.PriceChangeRequests
                        .Where(r => r.Status == PriceChangeRequestStatus.Pending)
                        .CountAsync(stoppingToken);

                    _logger.LogInformation("Processed pending price change requests. Remaining pending: {Count}", pendingCount);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    // graceful shutdown
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while processing price change requests.");
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // stoppingToken signaled
                }
            }

            _logger.LogInformation("PriceChangeProcessorWorker stopping.");
        }
    }
}
