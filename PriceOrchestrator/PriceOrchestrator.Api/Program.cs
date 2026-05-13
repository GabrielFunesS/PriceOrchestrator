
using PriceOrchestrator.Api.Endpoints;
using PriceOrchestrator.Api.Extensions;

namespace PriceOrchestrator.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
