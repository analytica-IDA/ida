using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Security.Claims;

namespace backend.Tests
{
    public class IntegrationTestBase : IDisposable
    {
        protected readonly HttpClient _client;
        protected readonly WebApplicationFactory<Program> _factory;

        public IntegrationTestBase()
        {
            var dbName = Guid.NewGuid().ToString();

            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    // Remove existing DbContext
                    services.RemoveAll(typeof(DbContextOptions<AppDbContext>));

                    // Add InMemory for testing
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(dbName);
                    });

                    // Add a policy that always succeeds for tests
                    services.AddAuthorization(options =>
                    {
                        options.DefaultPolicy = new AuthorizationPolicyBuilder()
                            .RequireAssertion(_ => true)
                            .Build();
                    });

                    // Ensure the database is created and seeded (via HasData)
                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        db.Database.EnsureCreated();
                    }
                });
            });

            _client = _factory.CreateClient();
        }

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
