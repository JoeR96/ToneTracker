using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ToneTracker.TestLib;

public class ApiClientFactory : WebApplicationFactory<Program>
{ 
    public static readonly WebApplicationFactory<Program> Factory = new WebApplicationFactory<Program>();

    public static async Task<(ToneTrackerClient, Guid)> CreateToneTrackerClientAsync(bool withToken = false)
    {
        var client = Factory.CreateClient();
        var userId = Guid.Empty;

        if (withToken)
        {
            var authClient = CreateAuthClientAsync();
            var authResponse = await authClient.LoginAsync(new LoginRequest
            {
                Email = "joeyrichardson96@gmail.com",
                Password = "Zelfdwnq9512!"
            });
            var token = authResponse.AccessToken;
            userId = authResponse.UserId;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return (new ToneTrackerClient(client.BaseAddress.AbsoluteUri,client), userId);
    }

    private static  AuthClient CreateAuthClientAsync()
    {
        var client = new HttpClient();
        const string baseUrl = "http://3.10.176.181:5001";
        return new AuthClient(baseUrl, client);
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the app's ApplicationDbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<ToneTrackerDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add ApplicationDbContext using an in-memory database for testing
            services.AddDbContext<ToneTrackerDbContext>(options =>
            {
                options.UseSqlite("Filename=:memory:");
            });

            // Build the service provider
            var serviceProvider = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database context
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ToneTrackerDbContext>();

            // Ensure the database is created
            db.Database.EnsureCreated();
            db.Database.Migrate();

        });
    }

   
    
}