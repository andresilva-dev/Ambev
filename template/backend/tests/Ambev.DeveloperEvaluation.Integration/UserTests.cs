using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration
{
    public class UserTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly DefaultContext _dbContext;

        public UserTests(WebApplicationFactory<Program> factory)
        {
            var webAppFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));
                    if (descriptor != null) services.Remove(descriptor);

                    services.AddDbContext<DefaultContext>(options =>
                    {
                        options.UseInMemoryDatabase("UserTestDb");
                    });
                });
            });

            _client = webAppFactory.CreateClient();
            _dbContext = webAppFactory.Services.CreateScope().ServiceProvider.GetRequiredService<DefaultContext>();

            SeedUsersAsync().GetAwaiter().GetResult();
        }

        private async Task SeedUsersAsync()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);

            var admin = new User
            {
                Username = "Admin User",
                Email = "admin@domain.com",
                Password = "$2a$11$zbm4DWcKa6/NqU0fTRpHlesxIKyGzpznA6JOtyqsYQi8YNyzTKFNK", // "123456"
                Phone = "(11) 99999-0000",
                Role = UserRole.Admin,
                Status = UserStatus.Active
            };

            _dbContext.Users.Add(admin);
            await _dbContext.SaveChangesAsync();
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

        [Fact(DisplayName = "POST /api/users creates user successfully")]
        public async Task CreateUser_ShouldReturnCreated()
        {
            var token = await AuthenticateAsync("admin@domain.com", "123456");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new CreateUserRequest
            {
                Username = "Integration Test User",
                Email = "testuser@domain.com",
                Password = "Adf$142@ft",
                Phone = "11912341234",
                Status = UserStatus.Active,
                Role = UserRole.Manager
            };

            var response = await _client.PostAsJsonAsync("/api/users", request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "GET /api/users/{id} returns user")]
        public async Task GetUserById_ShouldReturnUser()
        {
            var token = await AuthenticateAsync("admin@domain.com", "123456");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new CreateUserRequest
            {
                Username = "Get User",
                Email = "getuser@domain.com",
                Password = "As33f5t&",
                Phone = "11900000000",
                Role = UserRole.Manager,
                Status = UserStatus.Active
            };

            var createResponse = await _client.PostAsJsonAsync("/api/users", request);
            var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
            var id = created.GetProperty("data").GetProperty("id").GetGuid();

            var getResponse = await _client.GetAsync($"/api/users/{id}");

            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var getContent = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
            Assert.Equal(id, getContent.GetProperty("data").GetProperty("id").GetGuid());
        }

        [Fact(DisplayName = "DELETE /api/users/{id} removes user")]
        public async Task DeleteUser_ShouldSucceed()
        {
            var token = await AuthenticateAsync("admin@domain.com", "123456");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new CreateUserRequest
            {
                Username = "Delete User",
                Email = "deleteuser@domain.com",
                Password = "Ast5R$ghtt",
                Phone = "11955555555",
                Role = UserRole.Manager,
                Status = UserStatus.Active
            };

            var createResponse = await _client.PostAsJsonAsync("/api/users", request);
            var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
            var id = created.GetProperty("data").GetProperty("id").GetGuid();

            var deleteResponse = await _client.DeleteAsync($"/api/users/{id}");

            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}
