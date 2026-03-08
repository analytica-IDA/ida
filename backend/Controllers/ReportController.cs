using Microsoft.AspNetCore.Mvc;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("investment-roi")]
        public async Task<IActionResult> GetInvestmentRoi([FromQuery] long? idCliente)
        {
            var queryMeta = _context.ClientesInvestimentosMeta.AsQueryable();
            var queryGoogle = _context.ClientesInvestimentosGoogle.AsQueryable();

            if (idCliente.HasValue)
            {
                queryMeta = queryMeta.Where(m => m.IdCliente == idCliente.Value);
                queryGoogle = queryGoogle.Where(g => g.IdCliente == idCliente.Value);
            }

            var metaData = await queryMeta
                .OrderByDescending(m => m.DtUltimaAtualizacao)
                .Select(m => new { m.IdCliente, m.VlrInvestimentoMeta, m.DtUltimaAtualizacao })
                .ToListAsync();

            var googleData = await queryGoogle
                .OrderByDescending(g => g.DtUltimaAtualizacao)
                .Select(g => new { g.IdCliente, g.VlrInvestimentoGoogle, g.DtUltimaAtualizacao })
                .ToListAsync();

            return Ok(new { metaData, googleData });
        }

        [HttpGet("launch-distribution")]
        public async Task<IActionResult> GetLaunchDistribution([FromQuery] long? idCliente)
        {
            var varejoQuery = _context.LancamentosVarejo.AsQueryable();
            var cadastroQuery = _context.LancamentosCadastro.AsQueryable();
            var saudeQuery = _context.LancamentosSaude.AsQueryable();

            if (idCliente.HasValue)
            {
                // To filter by client, we need to join with ClientesUsuarios or check if the user is linked to the client
                var userIds = await _context.ClientesUsuarios
                    .Where(cu => cu.IdCliente == idCliente.Value)
                    .Select(cu => cu.IdUsuario)
                    .ToListAsync();

                varejoQuery = varejoQuery.Where(l => userIds.Contains(l.IdUsuario));
                cadastroQuery = cadastroQuery.Where(l => userIds.Contains(l.IdUsuario));
                saudeQuery = saudeQuery.Where(l => userIds.Contains(l.IdUsuario));
            }

            var varejoCount = await varejoQuery.CountAsync();
            var cadastroCount = await cadastroQuery.CountAsync();
            var saudeCount = await saudeQuery.CountAsync();

            return Ok(new[]
            {
                new { label = "Varejo", value = varejoCount },
                new { label = "Cadastro", value = cadastroCount },
                new { label = "Saúde", value = saudeCount }
            });
        }

        [HttpGet("user-productivity")]
        public async Task<IActionResult> GetUserProductivity([FromQuery] long? idCliente)
        {
            var userQuery = _context.Usuarios.AsQueryable();
            
            if (idCliente.HasValue)
            {
                var userIds = await _context.ClientesUsuarios
                    .Where(cu => cu.IdCliente == idCliente.Value)
                    .Select(cu => cu.IdUsuario)
                    .ToListAsync();
                userQuery = userQuery.Where(u => userIds.Contains(u.Id));
            }

            var productivity = await userQuery
                .Select(u => new
                {
                    u.Id,
                    UserName = u.Pessoa != null ? u.Pessoa.Nome : u.Login,
                    Clients = _context.ClientesUsuarios
                        .Where(cu => cu.IdUsuario == u.Id)
                        .Include(cu => cu.Cliente)
                        .Select(cu => cu.Cliente!.Nome)
                        .ToList(),
                    LaunchCount = _context.LancamentosVarejo.Count(l => l.IdUsuario == u.Id) +
                                   _context.LancamentosCadastro.Count(l => l.IdUsuario == u.Id) +
                                   _context.LancamentosSaude.Count(l => l.IdUsuario == u.Id)
                })
                .Where(p => p.LaunchCount > 0)
                .OrderByDescending(p => p.LaunchCount)
                .Take(10)
                .ToListAsync();

            return Ok(productivity);
        }

        [HttpGet("user-details/{idUsuario}")]
        public async Task<IActionResult> GetUserDetails(long idUsuario)
        {
            var varejo = await _context.LancamentosVarejo
                .Where(l => l.IdUsuario == idUsuario)
                .OrderByDescending(l => l.DataLancamento)
                .Take(5)
                .Select(l => new { Type = "Varejo", Date = l.DataLancamento, Summary = $"Vendas: {l.QtdInstagram + l.QtdFacebook + l.QtdGoogle}" })
                .ToListAsync();

            var cadastro = await _context.LancamentosCadastro
                .Where(l => l.IdUsuario == idUsuario)
                .OrderByDescending(l => l.DataLancamento)
                .Take(5)
                .Select(l => new { Type = "Cadastro", Date = l.DataLancamento, Summary = $"Cadastros: {l.QtdCadastros}" })
                .ToListAsync();

            var saude = await _context.LancamentosSaude
                .Where(l => l.IdUsuario == idUsuario)
                .OrderByDescending(l => l.DataLancamento)
                .Take(5)
                .Select(l => new { Type = "Saude", Date = l.DataLancamento, Summary = $"Redes: {l.QtdEntradaRedesSociais}, Google: {l.QtdEntradaGoogle}" })
                .ToListAsync();

            var all = varejo.Concat(cadastro).Concat(saude)
                .OrderByDescending(a => a.Date)
                .Take(15);

            return Ok(all);
        }
    }
}
