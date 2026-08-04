
using Microsoft.AspNetCore.Http;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation;
using PriceOrchestrator.Api.Endpoints;
using PriceOrchestrator.Api.Extensions;
using PriceOrchestrator.Api.Services;

namespace PriceOrchestrator.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<PriceService>();
            builder.Services.AddScoped<Services.Interfaces.IStockService, Services.StockService>();
            builder.Services.AddScoped<Services.Interfaces.IPromotionService, Services.PromotionService>();
            builder.Services.AddScoped<Services.Interfaces.IPriceChangeRequestService, Services.PriceChangeRequestService>();
            builder.Services.AddScoped<Services.Interfaces.IPriceService, Services.PriceService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // FluentValidation integration
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            // Register hosted worker
            builder.Services.AddHostedService<PriceOrchestrator.Api.Hosted.PriceChangeProcessorWorker>();

            builder.Services.AddDatabase(builder.Configuration);

            var app = builder.Build();

            app.MapProductsEndpoints();
            app.MapStockEndpoints();
            app.MapPromotionsEndpoints();
            app.MapPriceChangeEndpoints();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.Run();
        }
    }
}
