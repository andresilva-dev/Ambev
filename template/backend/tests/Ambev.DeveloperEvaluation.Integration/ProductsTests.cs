using Ambev.DeveloperEvaluation.Domain.Entities;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Microsoft.AspNetCore.Mvc.Testing;
using Ambev.DeveloperEvaluation.Application.Interfaces.Services;
using Ambev.DeveloperEvaluation.Tests.Commom;


namespace Ambev.DeveloperEvaluation.Integration
{
    public class ProductsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly DefaultContext _dbContext;

        public ProductsTests(WebApplicationFactory<Program> factory)
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

        private async Task SeedUsersAsync()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);

            var userAdmin = new User
            {
                Username = "Admin User",
                Email = "admin@domain.com",
                Password = "$2a$11$zbm4DWcKa6/NqU0fTRpHlesxIKyGzpznA6JOtyqsYQi8YNyzTKFNK",
                Phone = "(11) 91234-5678",
                Role = UserRole.Admin,
                Status = UserStatus.Active
            };

            _dbContext.Users.AddRange(
                userAdmin
            );

            await _dbContext.SaveChangesAsync();
        }

        private async Task<string> AuthenticateAsync(string email,string password)
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

        [Fact(DisplayName = "POST /api/products creates product successfully")]
        public async Task CreateProduct_ShouldReturnCreated()
        {
            var token = await AuthenticateAsync("admin@domain.com", "123456");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new CreateProductRequest
            {
                Name = "Test Product",
                Description = "Integration test description",
                UnitPrice = 29.99m
            };

            var response = await _client.PostAsJsonAsync("/api/products", request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "GET /api/products returns paginated list")]
        public async Task GetProducts_ShouldReturnList()
        {
            var response = await _client.GetAsync("/api/products?pageNumber=1&pageSize=5");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "GET /api/products/{id} returns product by ID")]
        public async Task GetProductById_ShouldReturnProduct()
        {
            var token = await AuthenticateAsync("admin@domain.com", "123456");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var createRequest = new CreateProductRequest
            {
                Name = "Unique Product",
                Description = "Get by ID",
                UnitPrice = 19.99m
            };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
            var id = created.GetProperty("data").GetProperty("id").GetGuid();

            var getResponse = await _client.GetAsync($"/api/products/{id}");

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var getContent = await getResponse.Content.ReadFromJsonAsync<JsonElement>();

            Assert.Equal(getContent.GetProperty("data").GetProperty("id").GetGuid(), id);
        }

        [Fact(DisplayName = "PUT /api/products updates product")]
        public async Task UpdateProduct_ShouldSucceed()
        {
            var token = await AuthenticateAsync("admin@domain.com", "123456");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var createRequest = new CreateProductRequest
            {
                Name = "To Update",
                Description = "Before update",
                UnitPrice = 15.00m
            };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
            var id = created.GetProperty("data").GetProperty("id").GetGuid();

            var updateRequest = new UpdateProductRequest
            {
                Id = id,
                Name = "Updated Name",
                Description = "Updated Desc",
                UnitPrice = 25.00m
            };

            var updateResponse = await _client.PutAsJsonAsync("/api/products", updateRequest);

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        }

        [Fact(DisplayName = "DELETE /api/products/{id} removes product")]
        public async Task DeleteProduct_ShouldSucceed()
        {
            var token = await AuthenticateAsync("admin@domain.com", "123456");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var createRequest = new CreateProductRequest
            {
                Name = "To Delete",
                Description = "Delete test",
                UnitPrice = 9.99m
            };
            var createResponse = await _client.PostAsJsonAsync("/api/products", createRequest);
            var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
            var id = created.GetProperty("data").GetProperty("id").GetGuid();

            var deleteResponse = await _client.DeleteAsync($"/api/products/{id}");

            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}
