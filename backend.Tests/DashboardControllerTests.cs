using backend.Controllers;
using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public DashboardControllerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
            var result = await controller.GetVarejoStats(502, null, null, null);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().NotBeNull();
            
            var faturamento = okResult.Value.GetType().GetProperty("totalFaturamento")?.GetValue(okResult.Value);
            faturamento.Should().Be(1000m);
        }

        [Fact]
        public async Task GetVarejoStats_WithAreaId_FiltersCorrectUsers()
        {
            // Arrange
            var controller = CreateController("1", "0"); // Admin
            var idCliente = 502L;
            var idArea1 = 110L;
            var idArea2 = 111L;

            _context.Areas.AddRange(
                new Area { Id = idArea1, Nome = "Area 1" },
                new Area { Id = idArea2, Nome = "Area 2" }
            );

            // User 1 in Area 1
            _context.Pessoas.Add(new Pessoa { Id = 1601, Nome = "P1", Cpf = "601", Email = "1", Telefone = "1", IdCliente = idCliente });
            _context.Usuarios.Add(new Usuario { Id = 1601, Login = "U1", IdCargo = 1, FlAtivo = true });
            _context.UsuariosAreas.Add(new UsuarioArea { IdUsuario = 1601, IdArea = idArea1 });
            _context.ClientesUsuarios.Add(new ClienteUsuario { Id = 1601, IdCliente = idCliente, IdUsuario = 1601, IdArea = idArea1 });
            
            _context.LancamentosVarejo.Add(new LancamentoVarejo 
            { 
                Id = 1601, 
                IdUsuario = 1601, 
                Faturamento = 1000, 
                IdModeloControle = 2, 
                DataLancamento = DateTime.Now 
            });

            // User 2 in Area 2
            _context.Pessoas.Add(new Pessoa { Id = 1602, Nome = "P2", Cpf = "602", Email = "2", Telefone = "2", IdCliente = idCliente });
            _context.Usuarios.Add(new Usuario { Id = 1602, Login = "U2", IdCargo = 1, FlAtivo = true });
            _context.UsuariosAreas.Add(new UsuarioArea { IdUsuario = 1602, IdArea = idArea2 });
            _context.ClientesUsuarios.Add(new ClienteUsuario { Id = 1602, IdCliente = idCliente, IdUsuario = 1602, IdArea = idArea2 });
            
            _context.LancamentosVarejo.Add(new LancamentoVarejo 
            { 
                Id = 1602, 
                IdUsuario = 1602, 
                Faturamento = 5000, 
                IdModeloControle = 2, 
                DataLancamento = DateTime.Now 
            });

            _context.SaveChanges();

            // Act - Request only Area 1
            var resultArea1 = await controller.GetVarejoStats(idCliente, idArea1, null, null);

            // Assert Area 1
            var okResult1 = resultArea1.Should().BeOfType<OkObjectResult>().Subject;
            var faturamento1 = okResult1.Value.GetType().GetProperty("totalFaturamento")?.GetValue(okResult1.Value);
            faturamento1.Should().Be(1000m);

            // Act - Request only Area 2
            var resultArea2 = await controller.GetVarejoStats(idCliente, idArea2, null, null);

            // Assert Area 2
            var okResult2 = resultArea2.Should().BeOfType<OkObjectResult>().Subject;
            var faturamento2 = okResult2.Value.GetType().GetProperty("totalFaturamento")?.GetValue(okResult2.Value);
            faturamento2.Should().Be(5000m);

            // Act - Request without area
            var resultAll = await controller.GetVarejoStats(idCliente, null, null, null);

            // Assert All
            var okResultAll = resultAll.Should().BeOfType<OkObjectResult>().Subject;
            var faturamentoAll = okResultAll.Value.GetType().GetProperty("totalFaturamento")?.GetValue(okResultAll.Value);
            faturamentoAll.Should().Be(6000m);
            
            var areaBreakdown = okResultAll.Value.GetType().GetProperty("areaBreakdown")?.GetValue(okResultAll.Value) as System.Collections.IEnumerable;
            areaBreakdown.Should().NotBeNull();
            
            var sellerBreakdown = okResultAll.Value.GetType().GetProperty("sellerBreakdown")?.GetValue(okResultAll.Value) as System.Collections.IEnumerable;
            sellerBreakdown.Should().NotBeNull();
        }

        [Fact]
        public async Task GetVarejoStats_RemovesDirtyRecords_Successfully()
        {
            // Arrange
            var controller = CreateController("1", "501");
            var idCliente = 501L;

            // User with NO area - Unique IDs (700+)
            _context.Pessoas.Add(new Pessoa { Id = 710, Nome = "NoAreaUser", Cpf = "710", Email = "1", Telefone = "1", IdCliente = idCliente });
            _context.Usuarios.Add(new Usuario { Id = 710, Login = "NoAreaUser", IdCargo = 1, FlAtivo = true });
            
            _context.LancamentosVarejo.Add(new LancamentoVarejo 
            { 
                Id = 710, 
                IdUsuario = 710, 
                Faturamento = 1000, 
                IdModeloControle = 2, 
                DataLancamento = DateTime.Now 
            });

            _context.SaveChanges();

            await controller.GetVarejoStats(idCliente, null, null, null);

            // Assert
            var launch = await _context.LancamentosVarejo.FindAsync(710L);
            launch.Should().BeNull(); // Should have been deleted as "dirty"
        }

        [Fact]
        public async Task GetVarejoStats_RemovesOrphanedInvestments_Successfully()
        {
            // Arrange
            var controller = CreateController("1", "501");
            var idCliente = 501L;

            _context.ClientesInvestimentosMeta.Add(new ClienteInvestimentoMeta 
            { 
                Id = 811, 
                IdCliente = idCliente, 
                VlrInvestimentoMeta = 100, 
                IdArea = 0, // Orphan
                DataReferencia = DateTime.Now 
            });

            _context.SaveChanges();

            await controller.GetVarejoStats(idCliente, null, null, null);

            // Assert
            var orphanedInv = await _context.ClientesInvestimentosMeta.FirstOrDefaultAsync(i => i.Id == 811);
            orphanedInv.Should().BeNull(); // Should have been deleted
        }

        [Fact]
        public async Task GetVarejoStats_SellerBreakdown_IncludesAreaName()
        {
            // Arrange
            var controller = CreateController("1", "501");
            var idCliente = 501L;
            var idArea = 120L;

            _context.Areas.Add(new Area { Id = idArea, Nome = "Area X" });
            _context.Pessoas.Add(new Pessoa { Id = 911, Nome = "Seller X", Cpf = "911", Email = "1", Telefone = "1", IdCliente = idCliente });
            _context.Usuarios.Add(new Usuario { Id = 911, Login = "SellerX", IdCargo = 1, FlAtivo = true });
            _context.UsuariosAreas.Add(new UsuarioArea { IdUsuario = 911, IdArea = idArea });
            _context.ClientesUsuarios.Add(new ClienteUsuario { IdUsuario = 911, IdCliente = idCliente, IdArea = idArea });

            _context.LancamentosVarejo.Add(new LancamentoVarejo 
            { 
                Id = 911, 
                IdUsuario = 911, 
                Faturamento = 1000, 
                IdModeloControle = 2, 
                DataLancamento = DateTime.Now 
            });

            _context.SaveChanges();

            // Act
            var result = await controller.GetVarejoStats(idCliente, null, null, null);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var sellerBreakdown = okResult.Value.GetType().GetProperty("sellerBreakdown")?.GetValue(okResult.Value) as System.Collections.IEnumerable;
            
            foreach (var item in sellerBreakdown)
            {
                var areaName = item.GetType().GetProperty("Area")?.GetValue(item) as string;
                if ((item.GetType().GetProperty("Name")?.GetValue(item) as string) == "Seller X")
                {
                    areaName.Should().Be("Area X");
                }
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
