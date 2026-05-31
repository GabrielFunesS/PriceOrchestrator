
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

            builder.Services.AddDatabase(builder.Configuration);

            var app = builder.Build();

            app.MapProductsEndpoints();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.Run();
        }
    }
}
