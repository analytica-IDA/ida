using backend.Controllers;
using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace backend.Tests
{
    public class DashboardControllerTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly SqliteConnection _connection;

        public DashboardControllerTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();
            
            // Seed basic data for tests
            SeedData();
        }

        private void SeedData()
        {
            // Only seed data not already in AppDbContext.cs HasData
            
            _context.Clientes.AddRange(
                new Cliente { Id = 501, Nome = "Cliente Alpha", Cnpj = "111", Email = "a@a.com", Telefone = "111" },
                new Cliente { Id = 502, Nome = "Cliente Beta", Cnpj = "222", Email = "b@b.com", Telefone = "222" }
            );

            _context.ClientesModelosControles.AddRange(
                new ClienteModeloControle { Id = 501, IdCliente = 501, IdModeloControle = 1 }, // Alpha has Cadastros
                new ClienteModeloControle { Id = 502, IdCliente = 501, IdModeloControle = 2 }  // Alpha has Varejo
            );

            _context.SaveChanges();
        }

        private DashboardController CreateController(string roleId, string idCliente)
        {
            var claims = new List<Claim>
            {
                new Claim("roleId", roleId),
                new Claim("idCliente", idCliente)
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var controller = new DashboardController(_context);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };

            return controller;
        }

        [Fact]
        public async Task GetAccessibleModels_Admin_ReturnsAllModels()
        {
            // Arrange
            var controller = CreateController("1", "0");

            // Act
            var result = await controller.GetAccessibleModels();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var models = okResult.Value.Should().BeAssignableTo<IEnumerable<string>>().Subject;
            models.Should().HaveCount(3);
            models.Should().Contain(new[] { "Cadastros", "Varejo", "Saúde" });
        }

        [Fact]
        public async Task GetAccessibleModels_Owner_ReturnsOnlyAuthorizedModels()
        {
            // Arrange
            var controller = CreateController("2", "501"); // Owner of Cliente 501 (Alpha)

            // Act
            var result = await controller.GetAccessibleModels();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var models = okResult.Value.Should().BeAssignableTo<IEnumerable<string>>().Subject;
            models.Should().HaveCount(2);
            models.Should().Contain(new[] { "Cadastros", "Varejo" });
            models.Should().NotContain("Saúde");
        }

        [Fact]
        public async Task GetVarejoStats_NonAdmin_EnforcesOwnClientId()
        {
            // Arrange
            var controller = CreateController("2", "501"); // Owner of Cliente 501
            
            // Add some data for Cliente 501 and Cliente 502
            _context.Pessoas.Add(new Pessoa { Id = 510, Nome = "P1", Cpf = "510", Email = "1", Telefone = "1", IdCliente = 501 });
            _context.Usuarios.Add(new Usuario { Id = 510, Login = "U1", IdCargo = 1, FlAtivo = true });
            _context.ClientesUsuarios.Add(new ClienteUsuario { Id = 510, IdCliente = 501, IdUsuario = 510, IdArea = 1 });
            
            _context.LancamentosVarejo.Add(new LancamentoVarejo 
            { 
                Id = 501, 
                IdUsuario = 510, 
                Faturamento = 1000, 
                IdModeloControle = 2, // Varejo
                DataLancamento = DateTime.Now 
            });

            _context.Pessoas.Add(new Pessoa { Id = 520, Nome = "P2", Cpf = "520", Email = "2", Telefone = "2", IdCliente = 502 });
            _context.Usuarios.Add(new Usuario { Id = 520, Login = "U2", IdCargo = 1, FlAtivo = true });
            _context.ClientesUsuarios.Add(new ClienteUsuario { Id = 520, IdCliente = 502, IdUsuario = 520, IdArea = 1 });
            
            _context.LancamentosVarejo.Add(new LancamentoVarejo 
            { 
                Id = 502, 
                IdUsuario = 520, 
                Faturamento = 5000, 
                IdModeloControle = 2, // Varejo
                DataLancamento = DateTime.Now 
            });

            _context.SaveChanges();

            // Act
            // Attempting to request idCliente=502, but claims are for idCliente=501
            var result = await controller.GetVarejoStats(502);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().NotBeNull();
            
            var faturamento = okResult.Value.GetType().GetProperty("totalFaturamento")?.GetValue(okResult.Value);
            faturamento.Should().Be(1000m);
        }

        [Fact]
        public async Task GetVarejoStats_Admin_AllowsAnyClientId()
        {
            // Arrange
            var controller = CreateController("1", "0"); // Admin
            
            // Data for Cliente 502
            _context.Pessoas.Add(new Pessoa { Id = 530, Nome = "P3", Cpf = "530", Email = "3", Telefone = "3", IdCliente = 502 });
            _context.Usuarios.Add(new Usuario { Id = 530, Login = "U3", IdCargo = 1, FlAtivo = true });
            _context.ClientesUsuarios.Add(new ClienteUsuario { Id = 530, IdCliente = 502, IdUsuario = 530, IdArea = 1 });
            
            _context.LancamentosVarejo.Add(new LancamentoVarejo 
            { 
                Id = 503, 
                IdUsuario = 530, 
                Faturamento = 8000, 
                IdModeloControle = 2, // Varejo
                DataLancamento = DateTime.Now 
            });

            _context.SaveChanges();

            // Act
            var result = await controller.GetVarejoStats(502);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().NotBeNull();
            
            var faturamento = okResult.Value.GetType().GetProperty("totalFaturamento")?.GetValue(okResult.Value);
            faturamento.Should().Be(8000m); 
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }
    }
}
