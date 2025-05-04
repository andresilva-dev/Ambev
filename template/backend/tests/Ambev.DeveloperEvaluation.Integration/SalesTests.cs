using System.Net;
using System.Text.Json;
using System.Net.Http.Json;
using Xunit;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Microsoft.AspNetCore.Mvc.Testing;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.ORM;
using Microsoft.Extensions.DependencyInjection;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Interfaces.Services;
using Ambev.DeveloperEvaluation.Tests.Commom;

namespace MyProject.IntegrationTests.Controllers;

public class SalesTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly DefaultContext _dbContext;
    private Guid _customerId;

    public SalesTests(WebApplicationFactory<Program> factory)
    {
        var webAppFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));
                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<DefaultContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                services.AddSingleton<ICacheService, FakeCacheService>();
            });
        });

        _client = webAppFactory.CreateClient();
        _dbContext = webAppFactory.Services.CreateScope().ServiceProvider.GetRequiredService<DefaultContext>();

        SeedUsersAsync().GetAwaiter().GetResult();
    }

    private async Task<string> AuthenticateAsync(string email, string password)
    {
        var response = await _client.PostAsJsonAsync("/api/auth", new AuthenticateUserRequest
        {
            Email = email,
            Password = password
        });

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty("data").GetProperty("token").GetString();
    }

    private async Task SeedUsersAsync()
    {
        _dbContext.Users.RemoveRange(_dbContext.Users);

        var userCustomer = new User
        {
            Username = "Customer User",
            Email = "customer@domain.com",
            Password = "$2a$11$zbm4DWcKa6/NqU0fTRpHlesxIKyGzpznA6JOtyqsYQi8YNyzTKFNK",
            Phone = "11912345678",
            Role = UserRole.Customer,
            Status = UserStatus.Active
        };

        var userAdmin = new User
        {
            Username = "Admin User",
            Email = "admin@domain.com",
            Password = "$2a$11$zbm4DWcKa6/NqU0fTRpHlesxIKyGzpznA6JOtyqsYQi8YNyzTKFNK",
            Phone = "11912345678",
            Role = UserRole.Admin,
            Status = UserStatus.Active
        };

        _dbContext.Users.AddRange(
            userCustomer, userAdmin
        );

        await _dbContext.SaveChangesAsync();
        _customerId = userCustomer.Id;
    }

    [Fact(DisplayName = "POST /api/sale creates sale successfully")]
    public async Task CreateSale_ShouldReturnCreated()
    {
        var createProductRequest = new CreateProductRequest
        {
            Name = "Produto Teste Venda",
            Description = "Produto para testes de venda",
            UnitPrice = 10.0m
        };

        var token = await AuthenticateAsync("admin@domain.com", "123456");
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _client.PostAsJsonAsync("/api/products", createProductRequest);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var productJson = await response.Content.ReadFromJsonAsync<JsonElement>();
        var productId = productJson.GetProperty("data").GetProperty("id").GetGuid();

        var saleRequest = new CreateSaleRequest
        {
            CustomerId = _customerId,
            Branch = "Test Branch",
            Items = new List<CreateSaleItemRequest>
            {
                new() { ProductId = productId, Quantity = 2 }
            }
        };

        token = await AuthenticateAsync("customer@domain.com", "123456");
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        response = await _client.PostAsJsonAsync("/api/sale", saleRequest);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
