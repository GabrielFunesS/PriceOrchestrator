using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PriceOrchestrator.Api.Services.Interfaces;

namespace PriceOrchestrator.Worker.Workers
{
    public class PriceChangeProcessorWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PriceChangeProcessorWorker> _logger;

        public PriceChangeProcessorWorker(
            IServiceScopeFactory scopeFactory,
            ILogger<PriceChangeProcessorWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PriceChangeProcessorWorker started.");

            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("PriceChangeProcessorWorker iteration started at {time}.", DateTime.UtcNow);
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var processor = scope.ServiceProvider.GetService<IPriceChangeRequestService>();

                    if (processor is null)
                    {
                        _logger.LogWarning("IPriceChangeRequestService no registrado en contenedor de dependencias.");
                        continue;
                    }

                    // Invocar la lógica real de procesamiento
                    await processor.ProcessPendingAsync(stoppingToken);
                    _logger.LogInformation("PriceChangeProcessorWorker processed batch.");
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("PriceChangeProcessorWorker cancellation requested.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error procesando cambios de precio.");
                }

                _logger.LogInformation("PriceChangeProcessorWorker iteration finished at {time}.", DateTime.UtcNow);
            }

            _logger.LogInformation("PriceChangeProcessorWorker stopped.");
        }
    }
}
