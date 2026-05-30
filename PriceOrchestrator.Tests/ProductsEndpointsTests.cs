using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PriceOrchestrator.Api.Data.Context;
using PriceOrchestrator.Api.DTOs;
using FluentAssertions;
using Xunit;

namespace PriceOrchestrator.Tests;

public class ProductsEndpointsTests : IClassFixture<WebApplicationFactory<PriceOrchestrator.Api.Program>>
{
    private readonly WebApplicationFactory<PriceOrchestrator.Api.Program> _factory;

    public ProductsEndpointsTests(WebApplicationFactory<PriceOrchestrator.Api.Program> factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClientWithInMemoryDb(string dbName)
    {
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase(dbName);
                });
            });
        }).CreateClient();

        return client;
    }

    [Fact]
    public async Task GetAll_ReturnsOkAndEmptyList_WhenNoProducts()
    {
        var client = CreateClientWithInMemoryDb("testdb1");

        var response = await client.GetAsync("/api/products/");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        products.Should().NotBeNull();
        products.Should().BeEmpty();
    }

    [Fact]
    public async Task Post_CreateProduct_ReturnsCreatedAndProduct()
    {
        var client = CreateClientWithInMemoryDb("testdb2");

        var request = new {
            ExternalId = "ext-1",
            Name = "Product 1",
            Description = "Desc",
            Brand = "Brand",
            Category = "Cat"
        };

        var response = await client.PostAsJsonAsync("/api/products/", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await response.Content.ReadFromJsonAsync<ProductDto>();
        created.Should().NotBeNull();
        created!.ExternalId.Should().Be("ext-1");
        created.Name.Should().Be("Product 1");
    }
}
