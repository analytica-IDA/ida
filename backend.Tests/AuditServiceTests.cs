using backend.Data;
using backend.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace backend.Tests
{
    public class AuditServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly AuditService _auditService;

        public AuditServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();

            _auditService = new AuditService(_context);
            
            SeedData();
        }

        private void SeedData()
        {
            _context.Clientes.Add(new Cliente { Id = 100, Nome = "TestClient", Cnpj = "1", Email = "1", Telefone = "1" });

            _context.Pessoas.AddRange(
                new Pessoa { Id = 901, Nome = "Admin User", Cpf = "1", Email = "1", Telefone = "1" },
                new Pessoa { Id = 902, Nome = "Owner User", Cpf = "2", Email = "2", Telefone = "2", IdCliente = 100 },
                new Pessoa { Id = 904, Nome = "Vendedor User", Cpf = "4", Email = "4", Telefone = "4", IdCliente = 100 }
            );

            _context.Usuarios.AddRange(
                new Usuario { Id = 901, Login = "admin", IdCargo = 1, FlAtivo = true },
                new Usuario { Id = 902, Login = "owner", IdCargo = 2, FlAtivo = true },
                new Usuario { Id = 904, Login = "vendedor", IdCargo = 4, FlAtivo = true }
            );

            _context.SaveChanges();
        }

        [Fact]
        public async Task LogAction_Login_DoesNotCreateNotification()
        {
            // Act
            await _auditService.LogAction("vendedor", "LOGIN", "usuario", "{}", "{}");

            // Assert
            var notifications = await _context.Notificacoes.ToListAsync();
            notifications.Should().BeEmpty();
        }

        [Fact]
        public async Task LogAction_Create_CreatesNotificationForAdminAndOwner()
        {
            // Act
            await _auditService.LogAction("vendedor", "CREATE", "lancamento_varejo", "{}", "{\"id\":1}");

            // Assert
            // In-memory database with the same name shares data within the same process/run
            var notifications = await _context.Notificacoes.ToListAsync();
            notifications.Should().NotBeEmpty();
            notifications.Should().Contain(n => n.IdUsuarioDestino == 901); // Admin
            notifications.Should().Contain(n => n.IdUsuarioDestino == 902); // Owner of the same client
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
