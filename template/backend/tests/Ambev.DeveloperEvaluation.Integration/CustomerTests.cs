using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration
{
    public class CustomerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly DefaultContext _dbContext;

        public CustomerTests(WebApplicationFactory<Program> factory)
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
                });
            });

            _client = webAppFactory.CreateClient();
            _dbContext = webAppFactory.Services.CreateScope().ServiceProvider.GetRequiredService<DefaultContext>();

            SeedUsersAsync().GetAwaiter().GetResult();
        }

        private async Task SeedUsersAsync()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);

            _dbContext.Users.Add(new User
            {
                Username = "Admin",
                Email = "admin@domain.com",
                Password = "$2a$11$zbm4DWcKa6/NqU0fTRpHlesxIKyGzpznA6JOtyqsYQi8YNyzTKFNK",
                Phone = "(11) 91234-5678",
                Role = UserRole.Admin,
                Status = UserStatus.Active
            });

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

        [Fact(DisplayName = "POST /api/customers creates a customer")]
        public async Task CreateCustomer_ShouldReturnCreated()
        {
            var token = await AuthenticateAsync("admin@domain.com", "123456");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var request = new CreateCustomerRequest
            {
                Name = "John Doe",
                Cpf = "02241815055",
                Email = "john.doe@email.com",
                Status = CustomerStatus.Active
            };

            var response = await _client.PostAsJsonAsync("/api/customers", request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact(DisplayName = "GET /api/customers returns paginated list")]
        public async Task GetCustomers_ShouldReturnList()
        {
            var token = await AuthenticateAsync("admin@domain.com", "123456");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            
            var response = await _client.GetAsync("/api/customers?pageNumber=1&pageSize=5");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "GET /api/customers/{id} returns customer by ID")]
        public async Task GetCustomerById_ShouldReturnCustomer()
        {
            var token = await AuthenticateAsync("admin@domain.com", "123456");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var createRequest = new CreateCustomerRequest
            {
                Name = "Jane Doe",
                Cpf = "77834034090",
                Email = "jane.doe@email.com",
                Status = CustomerStatus.Active
            };

            var createResponse = await _client.PostAsJsonAsync("/api/customers", createRequest);
            var json = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
            var id = json.GetProperty("data").GetProperty("id").GetGuid();

            var response = await _client.GetAsync($"/api/customers/{id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "PUT /api/customers updates customer")]
        public async Task UpdateCustomer_ShouldSucceed()
        {
            var token = await AuthenticateAsync("admin@domain.com", "123456");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var createRequest = new CreateCustomerRequest
            {
                Name = "To Update",
                Cpf = "44966408059",
                Email = "toupdate@email.com",
                Status = CustomerStatus.Active
            };
            var createResponse = await _client.PostAsJsonAsync("/api/customers", createRequest);
            var json = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
            var id = json.GetProperty("data").GetProperty("id").GetGuid();

            var updateRequest = new UpdateCustomerRequest
            {
                Id = id,
                Name = "Updated Name",
                Cpf = "20535693010",
                Email = "updated@email.com",
                Status = CustomerStatus.Active
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/customers/{id}", updateRequest);

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        }

        [Fact(DisplayName = "DELETE /api/customers/{id} removes customer")]
        public async Task DeleteCustomer_ShouldSucceed()
        {
            var token = await AuthenticateAsync("admin@domain.com", "123456");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var createRequest = new CreateCustomerRequest
            {
                Name = "To Delete",
                Cpf = "14933271003",
                Email = "delete@email.com",
                Status = CustomerStatus.Active
            };
            var createResponse = await _client.PostAsJsonAsync("/api/customers", createRequest);
            var json = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
            var id = json.GetProperty("data").GetProperty("id").GetGuid();

            var deleteResponse = await _client.DeleteAsync($"/api/customers/{id}");

            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }
    }
}
