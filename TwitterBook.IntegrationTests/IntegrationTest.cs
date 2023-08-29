using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using TwitterBook.Contracts.V1;
using TwitterBook.Contracts.V1.Requests;
using TwitterBook.Contracts.V1.Response;
using TwitterBook.Data;
using TwitterBook.Domain;

namespace TwitterBook.IntegrationTests;

public class IntegrationTest : IDisposable
{
    protected readonly HttpClient TestClient;
    private readonly IServiceProvider _serviceProvider;
    protected IntegrationTest()
    {
        var appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(DataContext));
                    services.AddEntityFrameworkInMemoryDatabase();
                    services.AddDbContext<DataContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });
                });
            });
        _serviceProvider = appFactory.Services;
        TestClient = appFactory.CreateClient();
    }

    protected async Task AuthenticateAsync()
    {
        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
    }

    protected async Task<PostResponse> CreatePostAsync(CreatePostRequest request)
    {
      var response = await TestClient.PostAsJsonAsync(ApiRoutes.Posts.Create, request);
      return await response.Content.ReadAsAsync<PostResponse>();
    }
    
    private async Task<string> GetJwtAsync()
    {
        var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest()
        {
            Email = "qwert@test.com",
            Password = "Blendi123."
        });
        var registrationResponse = await response.Content.ReadAsAsync<AuthSuccesResponse>();
        return registrationResponse.Token;
    }

    public void Dispose()
    {
        using var serviceScope = _serviceProvider.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<DataContext>();
        context.Database.EnsureDeleted();
    }
}