using Microsoft.AspNetCore.Mvc;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        private long GetCurrentRoleId() => long.Parse(User.FindFirst("roleId")?.Value ?? "0");
        private long GetCurrentClientId() => long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

        [HttpGet("accessible-models")]
        public async Task<IActionResult> GetAccessibleModels()
        {
            var roleId = GetCurrentRoleId();
            var idCliente = GetCurrentClientId();

            if (roleId == 1) // Admin
            {
                return Ok(new[] { "Cadastros", "Varejo", "Saúde" });
            }

            var accessibleModels = await _context.ClientesModelosControles
                .Where(cmc => cmc.IdCliente == idCliente)
                .Include(cmc => cmc.ModeloControle)
                .Select(cmc => cmc.ModeloControle!.Nome)
                .ToListAsync();

            return Ok(accessibleModels);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats([FromQuery] long? idCliente)
        {
            var roleId = GetCurrentRoleId();
            var claimClientId = GetCurrentClientId();

            // Tenant boundary
            if (roleId != 1) idCliente = claimClientId;

            var usersQuery = _context.Usuarios.AsQueryable();
            var clientsQuery = _context.Clientes.AsQueryable();
            var personsQuery = _context.Pessoas.AsQueryable();

            if (idCliente.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.Pessoa!.IdCliente == idCliente.Value);
                clientsQuery = clientsQuery.Where(c => c.Id == idCliente.Value);
                personsQuery = personsQuery.Where(p => p.IdCliente == idCliente.Value);
            }

            var activeUsers = await usersQuery.CountAsync(u => u.FlAtivo);
            var partnerClients = await clientsQuery.CountAsync();
            var registeredPersons = await personsQuery.CountAsync();
            var reportsGenerated = await _context.LogsAuditoria.CountAsync(); // Audit logs usually global or we filter by user in client?

            return Ok(new
            {
                activeUsers,
                partnerClients,
                registeredPersons,
                reportsGenerated
            });
        }

        [HttpGet("varejo")]
        public async Task<IActionResult> GetVarejoStats([FromQuery] long? idCliente)
        {
            var roleId = GetCurrentRoleId();
            var claimClientId = GetCurrentClientId();

            if (roleId != 1) idCliente = claimClientId;

            var varejoQuery = _context.LancamentosVarejo.AsQueryable();
            var invMetaQuery = _context.ClientesInvestimentosMeta.AsQueryable();
            var invGoogleQuery = _context.ClientesInvestimentosGoogle.AsQueryable();

            if (idCliente.HasValue)
            {
                var userIds = await _context.ClientesUsuarios
                    .Where(cu => cu.IdCliente == idCliente.Value)
                    .Select(cu => cu.IdUsuario)
                    .ToListAsync();

                varejoQuery = varejoQuery.Where(l => userIds.Contains(l.IdUsuario));
                invMetaQuery = invMetaQuery.Where(i => i.IdCliente == idCliente.Value);
                invGoogleQuery = invGoogleQuery.Where(i => i.IdCliente == idCliente.Value);
            }

            var results = await varejoQuery.ToListAsync();
            
            var totalAtendimento = results.Sum(r => r.QtdAtendimento);
            var totalFechamento = results.Sum(r => r.QtdFechamento);
            var totalFaturamento = (decimal)await varejoQuery.Select(l => (double)l.Faturamento).SumAsync();
            var totalInstagram = results.Sum(r => r.QtdInstagram);
            var totalFacebook = results.Sum(r => r.QtdFacebook);
            var totalGoogle = results.Sum(r => r.QtdGoogle);
            var totalIndicacao = results.Sum(r => r.QtdIndicacao);

            var totalInvestimentoMeta = (decimal)await invMetaQuery.Select(i => (double)i.VlrInvestimentoMeta).SumAsync();
            var totalInvestimentoGoogle = (decimal)await invGoogleQuery.Select(i => (double)i.VlrInvestimentoGoogle).SumAsync();
            var totalInvestimento = totalInvestimentoMeta + totalInvestimentoGoogle;

            return Ok(new
            {
                totalAtendimento,
                totalFechamento,
                totalFaturamento,
                totalInstagram,
                totalFacebook,
                totalGoogle,
                totalIndicacao,
                totalInvestimentoMeta,
                totalInvestimentoGoogle,
                totalInvestimento,
                roas = totalInvestimento > 0 ? totalFaturamento / totalInvestimento : 0,
                conversionRate = totalAtendimento > 0 ? (decimal)totalFechamento / totalAtendimento : 0
            });
        }

        [HttpGet("saude")]
        public async Task<IActionResult> GetSaudeStats([FromQuery] long? idCliente)
        {
            var roleId = GetCurrentRoleId();
            var claimClientId = GetCurrentClientId();

            if (roleId != 1) idCliente = claimClientId;

            var saudeQuery = _context.LancamentosSaude.AsQueryable();
            var invMetaQuery = _context.ClientesInvestimentosMeta.AsQueryable();
            var invGoogleQuery = _context.ClientesInvestimentosGoogle.AsQueryable();

            if (idCliente.HasValue)
            {
                var userIds = await _context.ClientesUsuarios
                    .Where(cu => cu.IdCliente == idCliente.Value)
                    .Select(cu => cu.IdUsuario)
                    .ToListAsync();

                saudeQuery = saudeQuery.Where(l => userIds.Contains(l.IdUsuario));
                invMetaQuery = invMetaQuery.Where(i => i.IdCliente == idCliente.Value);
                invGoogleQuery = invGoogleQuery.Where(i => i.IdCliente == idCliente.Value);
            }

            var results = await saudeQuery.ToListAsync();

            var totalClickMeta = results.Sum(r => r.QtdClickMeta);
            var totalClickGoogle = results.Sum(r => r.QtdClickGoogle);
            var totalContatosReais = results.Sum(r => r.QtdContatosReais);
            var totalConversaoConsultas = results.Sum(r => r.QtdConversaoConsultas);
            var totalEntradaRedesSociais = results.Sum(r => r.QtdEntradaRedesSociais);
            var totalEntradaGoogle = results.Sum(r => r.QtdEntradaGoogle);
            
            var totalFaturamento = (decimal)await saudeQuery.Select(l => (double)(l.QtdConversaoConsultas * (l.VlrTicketMedioConsultas ?? 0))).SumAsync();

            var totalInvestimentoMeta = (decimal)await invMetaQuery.Select(i => (double)i.VlrInvestimentoMeta).SumAsync();
            var totalInvestimentoGoogle = (decimal)await invGoogleQuery.Select(i => (double)i.VlrInvestimentoGoogle).SumAsync();
            var totalInvestimento = totalInvestimentoMeta + totalInvestimentoGoogle;

            return Ok(new
            {
                totalClickMeta,
                totalClickGoogle,
                totalContatosReais,
                totalConversaoConsultas,
                totalEntradaRedesSociais,
                totalEntradaGoogle,
                totalFaturamento,
                totalInvestimentoMeta,
                totalInvestimentoGoogle,
                totalInvestimento,
                roas = totalInvestimento > 0 ? totalFaturamento / totalInvestimento : 0,
                conversionRate = (totalClickMeta + totalClickGoogle) > 0 ? (decimal)totalContatosReais / (totalClickMeta + totalClickGoogle) : 0
            });
        }

        [HttpGet("cadastro")]
        public async Task<IActionResult> GetCadastroStats([FromQuery] long? idCliente)
        {
            var roleId = GetCurrentRoleId();
            var claimClientId = GetCurrentClientId();

            if (roleId != 1) idCliente = claimClientId;

            var cadastroQuery = _context.LancamentosCadastro.AsQueryable();
            var invMetaQuery = _context.ClientesInvestimentosMeta.AsQueryable();
            var invGoogleQuery = _context.ClientesInvestimentosGoogle.AsQueryable();

            if (idCliente.HasValue)
            {
                var userIds = await _context.ClientesUsuarios
                    .Where(cu => cu.IdCliente == idCliente.Value)
                    .Select(cu => cu.IdUsuario)
                    .ToListAsync();

                cadastroQuery = cadastroQuery.Where(l => userIds.Contains(l.IdUsuario));
                invMetaQuery = invMetaQuery.Where(i => i.IdCliente == idCliente.Value);
                invGoogleQuery = invGoogleQuery.Where(i => i.IdCliente == idCliente.Value);
            }

            var results = await cadastroQuery.ToListAsync();

            var totalClickLink = results.Sum(r => r.QtdClickLink);
            var totalCadastros = results.Sum(r => r.QtdCadastros);
            var totalFaturamento = (decimal)await cadastroQuery.Select(r => (double)(r.QtdCadastros * r.VlrTicketMedio)).SumAsync();

            var totalInvestimentoMeta = (decimal)await invMetaQuery.Select(i => (double)i.VlrInvestimentoMeta).SumAsync();
            var totalInvestimentoGoogle = (decimal)await invGoogleQuery.Select(i => (double)i.VlrInvestimentoGoogle).SumAsync();
            var totalInvestimento = totalInvestimentoMeta + totalInvestimentoGoogle;

            return Ok(new
            {
                totalClickLink,
                totalCadastros,
                totalFaturamento,
                totalInvestimentoMeta,
                totalInvestimentoGoogle,
                totalInvestimento,
                roas = totalInvestimento > 0 ? totalFaturamento / totalInvestimento : 0,
                conversionRate = totalClickLink > 0 ? (decimal)totalCadastros / totalClickLink : 0,
                cpa = totalCadastros > 0 ? totalInvestimento / totalCadastros : 0
            });
        }
    }
}
