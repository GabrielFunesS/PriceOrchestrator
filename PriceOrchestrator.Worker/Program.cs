using Microsoft.Extensions.Hosting;
using PriceOrchestrator.Api.Extensions;
using PriceOrchestrator.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);

// Registrar servicios y dependencias similares a la API
builder.Services.AddScoped<PriceOrchestrator.Api.Services.ProductService>();
builder.Services.AddScoped<PriceOrchestrator.Api.Services.PriceService>();
builder.Services.AddScoped<PriceOrchestrator.Api.Services.Interfaces.IStockService, PriceOrchestrator.Api.Services.StockService>();
builder.Services.AddScoped<PriceOrchestrator.Api.Services.Interfaces.IPromotionService, PriceOrchestrator.Api.Services.PromotionService>();
builder.Services.AddScoped<PriceOrchestrator.Api.Services.Interfaces.IPriceChangeRequestService, PriceOrchestrator.Api.Services.PriceChangeRequestService>();
builder.Services.AddScoped<PriceOrchestrator.Api.Services.Interfaces.IPriceService, PriceOrchestrator.Api.Services.PriceService>();

builder.Services.AddDatabase(builder.Configuration);

builder.Services.AddHostedService<PriceChangeProcessorWorker>();

var host = builder.Build();
host.Run();
