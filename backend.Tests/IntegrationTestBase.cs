using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
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
        private readonly SqliteConnection _connection;

        public IntegrationTestBase()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove existing DbContext
                    services.RemoveAll(typeof(DbContextOptions<AppDbContext>));

                    // Add SQLite for testing
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseSqlite(_connection);
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
                        db.Database.EnsureDeleted();
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
            _connection.Close();
            _connection.Dispose();
        }
    }
}
