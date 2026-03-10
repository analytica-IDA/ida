using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using backend.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using backend.Services;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LancamentoController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly INotificationService _notificationService;

        public LancamentoController(AppDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        private long GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (long.TryParse(userIdClaim, out var userId)) return userId;
            throw new Exception("User ID not found in token");
        }

        // --- GET Endpoints ---
        
        [HttpGet("cliente/{idCliente}")]
        public async Task<IActionResult> GetByCliente(long idCliente, [FromQuery] long? idModeloControle, [FromQuery] long? idArea, [FromQuery] DateTime? dataInicial, [FromQuery] DateTime? dataFinal)
        {
            var roleIdClaim = User.FindFirst("roleId")?.Value;
            var currentUserId = GetCurrentUserId();
            long roleId = 0;
            long.TryParse(roleIdClaim, out roleId);

            // Verify if client has this modelo
            var hasModel = await _context.ClientesModelosControles
                .AnyAsync(cm => cm.IdCliente == idCliente && cm.IdModeloControle == idModeloControle);

            if (!hasModel) return BadRequest(new { message = "Cliente não possui o modelo de controle informado." });

            // Base query logic
            if (idModeloControle == 1) // Cadastro
            {
                var query = _context.LancamentosCadastro
                    .Include(l => l.Usuario!.Pessoa)
                    .Include(l => l.ClienteInvestimentoMeta)
                    .Include(l => l.ClienteInvestimentoGoogle)
                    .Where(l => l.Usuario!.Pessoa!.IdCliente == idCliente)
                    .AsQueryable();

                if (roleId == 4) query = query.Where(l => l.IdUsuario == currentUserId);
                if (idArea.HasValue && idArea > 0)
                {
                    query = query.Where(l => 
                        (l.ClienteInvestimentoMeta != null && l.ClienteInvestimentoMeta.IdArea == idArea.Value) || 
                        (l.ClienteInvestimentoGoogle != null && l.ClienteInvestimentoGoogle.IdArea == idArea.Value) ||
                        (l.IdUsuario == currentUserId && roleId == 4) // For salesperson, show his own if no area yet
                    );
                }
                if (dataInicial.HasValue) query = query.Where(l => l.DataLancamento >= dataInicial.Value);
                if (dataFinal.HasValue) query = query.Where(l => l.DataLancamento <= dataFinal.Value.AddDays(1).AddTicks(-1));
                return Ok(await query.OrderByDescending(l => l.DataLancamento).ToListAsync());
            }
            else if (idModeloControle == 2) // Varejo
            {
                var query = _context.LancamentosVarejo
                    .Include(l => l.Usuario!.Pessoa)
                    .Include(l => l.ClienteInvestimentoMeta)
                    .Include(l => l.ClienteInvestimentoGoogle)
                    .Where(l => l.Usuario!.Pessoa!.IdCliente == idCliente)
                    .AsQueryable();

                if (roleId == 4) query = query.Where(l => l.IdUsuario == currentUserId);
                if (idArea.HasValue && idArea > 0)
                {
                    query = query.Where(l => 
                        (l.ClienteInvestimentoMeta != null && l.ClienteInvestimentoMeta.IdArea == idArea.Value) || 
                        (l.ClienteInvestimentoGoogle != null && l.ClienteInvestimentoGoogle.IdArea == idArea.Value) ||
                        (l.IdUsuario == currentUserId && roleId == 4)
                    );
                }
                if (dataInicial.HasValue) query = query.Where(l => l.DataLancamento >= dataInicial.Value);
                if (dataFinal.HasValue) query = query.Where(l => l.DataLancamento <= dataFinal.Value.AddDays(1).AddTicks(-1));
                return Ok(await query.OrderByDescending(l => l.DataLancamento).ToListAsync());
            }
            else if (idModeloControle == 3) // Saude
            {
                var query = _context.LancamentosSaude
                    .Include(l => l.Usuario!.Pessoa)
                    .Include(l => l.ClienteInvestimentoMeta)
                    .Include(l => l.ClienteInvestimentoGoogle)
                    .Where(l => l.Usuario!.Pessoa!.IdCliente == idCliente)
                    .AsQueryable();

                if (roleId == 4) query = query.Where(l => l.IdUsuario == currentUserId);
                if (idArea.HasValue && idArea > 0)
                {
                    query = query.Where(l => 
                        (l.ClienteInvestimentoMeta != null && l.ClienteInvestimentoMeta.IdArea == idArea.Value) || 
                        (l.ClienteInvestimentoGoogle != null && l.ClienteInvestimentoGoogle.IdArea == idArea.Value) ||
                        (l.IdUsuario == currentUserId && roleId == 4)
                    );
                }
                if (dataInicial.HasValue) query = query.Where(l => l.DataLancamento >= dataInicial.Value);
                if (dataFinal.HasValue) query = query.Where(l => l.DataLancamento <= dataFinal.Value.AddDays(1).AddTicks(-1));
                return Ok(await query.OrderByDescending(l => l.DataLancamento).ToListAsync());
            }

            return BadRequest("Modelo de controle não suportado.");
        }

        // --- POST Endpoints ---

        [HttpPost("varejo")]
        public async Task<IActionResult> CreateVarejo([FromBody] LancamentoVarejo dto)
        {
            var userId = GetCurrentUserId();
            dto.IdUsuario = userId;
            dto.IdModeloControle = 2; // Varejo
            
            // Link current investment if not provided
            if (dto.IdClienteInvestimentoMeta == null || dto.IdClienteInvestimentoMeta == 0)
            {
                var user = await _context.Usuarios.Include(u => u.Pessoa).FirstOrDefaultAsync(u => u.Id == userId);
                if (user?.Pessoa != null)
                {
                    var meta = await _context.ClientesInvestimentosMeta
                        .Where(m => m.IdCliente == user.Pessoa.IdCliente)
                        .OrderByDescending(m => m.DtUltimaAtualizacao)
                        .FirstOrDefaultAsync();
                    dto.IdClienteInvestimentoMeta = meta?.Id;
                }
            }

            if (dto.IdClienteInvestimentoGoogle == null || dto.IdClienteInvestimentoGoogle == 0)
            {
                var user = await _context.Usuarios.Include(u => u.Pessoa).FirstOrDefaultAsync(u => u.Id == userId);
                if (user?.Pessoa != null)
                {
                    var google = await _context.ClientesInvestimentosGoogle
                        .Where(g => g.IdCliente == user.Pessoa.IdCliente)
                        .OrderByDescending(g => g.DtUltimaAtualizacao)
                        .FirstOrDefaultAsync();
                    dto.IdClienteInvestimentoGoogle = google?.Id;
                }
            }
            
            _context.LancamentosVarejo.Add(dto);
            await _context.SaveChangesAsync();

            await NotifySuperiorsAndAdmins(userId, "Varejo");

            return Ok(dto);
        }

        private async Task NotifySuperiorsAndAdmins(long idUsuarioLogado, string modeloControleName)
        {
            var user = await _context.Usuarios
                .Include(u => u.Pessoa)
                .FirstOrDefaultAsync(u => u.Id == idUsuarioLogado);

            if (user == null || user.Pessoa == null) return;

            var idCliente = user.Pessoa.IdCliente;
            var nomeUsuario = user.Pessoa.Nome;

            var adminIds = await _context.Usuarios
                .Include(u => u.Cargo).ThenInclude(c => c!.Role)
                .Where(u => u.Cargo!.Role!.Nome.ToLower() == "admin")
                .Select(u => u.Id)
                .ToListAsync();

            var superiorIds = await _context.Usuarios
                .Include(u => u.Pessoa)
                .Include(u => u.Cargo).ThenInclude(c => c!.Role)
                .Where(u => u.Pessoa!.IdCliente == idCliente && 
                            (u.Cargo!.Role!.Nome.ToLower() == "proprietário" || 
                             u.Cargo.Role.Nome.ToLower() == "supervisor" ||
                             u.Cargo.Role.Nome.ToLower() == "proprietario") &&
                            u.Id != idUsuarioLogado)
                .Select(u => u.Id)
                .ToListAsync();

            var targetIds = adminIds.Union(superiorIds).Distinct();

            foreach (var targetId in targetIds)
            {
                await _notificationService.CreateNotification(
                    targetId, 
                    "Novo Lançamento", 
                    $"O usuário {nomeUsuario} registrou um novo lançamento de resultados para o modelo {modeloControleName}.", 
                    "Sistema");
            }
        }

        [HttpPost("cadastro")]
        public async Task<IActionResult> CreateCadastro([FromBody] LancamentoCadastro dto)
        {
            var userId = GetCurrentUserId();
            dto.IdUsuario = userId;
            dto.IdModeloControle = 1; // Cadastro

            // Link current investment if not provided
            if (dto.IdClienteInvestimentoMeta == null || dto.IdClienteInvestimentoMeta == 0)
            {
                var user = await _context.Usuarios.Include(u => u.Pessoa).FirstOrDefaultAsync(u => u.Id == userId);
                if (user?.Pessoa != null)
                {
                    var meta = await _context.ClientesInvestimentosMeta
                        .Where(m => m.IdCliente == user.Pessoa.IdCliente)
                        .OrderByDescending(m => m.DtUltimaAtualizacao)
                        .FirstOrDefaultAsync();
                    dto.IdClienteInvestimentoMeta = meta?.Id;
                }
            }

            if (dto.IdClienteInvestimentoGoogle == null || dto.IdClienteInvestimentoGoogle == 0)
            {
                var user = await _context.Usuarios.Include(u => u.Pessoa).FirstOrDefaultAsync(u => u.Id == userId);
                if (user?.Pessoa != null)
                {
                    var google = await _context.ClientesInvestimentosGoogle
                        .Where(g => g.IdCliente == user.Pessoa.IdCliente)
                        .OrderByDescending(g => g.DtUltimaAtualizacao)
                        .FirstOrDefaultAsync();
                    dto.IdClienteInvestimentoGoogle = google?.Id;
                }
            }
            
            _context.LancamentosCadastro.Add(dto);
            await _context.SaveChangesAsync();

            await NotifySuperiorsAndAdmins(userId, "Cadastros");

            return Ok(dto);
        }

        [HttpPost("saude")]
        public async Task<IActionResult> CreateSaude([FromBody] LancamentoSaude dto)
        {
            var userId = GetCurrentUserId();
            dto.IdUsuario = userId;
            dto.IdModeloControle = 3; // Saude

            // Link current investment if not provided
            if (dto.IdClienteInvestimentoMeta == null || dto.IdClienteInvestimentoMeta == 0)
            {
                var user = await _context.Usuarios.Include(u => u.Pessoa).FirstOrDefaultAsync(u => u.Id == userId);
                if (user?.Pessoa != null)
                {
                    var meta = await _context.ClientesInvestimentosMeta
                        .Where(m => m.IdCliente == user.Pessoa.IdCliente)
                        .OrderByDescending(m => m.DtUltimaAtualizacao)
                        .FirstOrDefaultAsync();
                    dto.IdClienteInvestimentoMeta = meta?.Id;
                }
            }

            if (dto.IdClienteInvestimentoGoogle == null || dto.IdClienteInvestimentoGoogle == 0)
            {
                var user = await _context.Usuarios.Include(u => u.Pessoa).FirstOrDefaultAsync(u => u.Id == userId);
                if (user?.Pessoa != null)
                {
                    var google = await _context.ClientesInvestimentosGoogle
                        .Where(g => g.IdCliente == user.Pessoa.IdCliente)
                        .OrderByDescending(g => g.DtUltimaAtualizacao)
                        .FirstOrDefaultAsync();
                    dto.IdClienteInvestimentoGoogle = google?.Id;
                }
            }
            
            _context.LancamentosSaude.Add(dto);
            await _context.SaveChangesAsync();

            await NotifySuperiorsAndAdmins(userId, "Saúde");

            return Ok(dto);
        }

        // --- PUT Endpoints ---

        [HttpPut("varejo/{id}")]
        public async Task<IActionResult> UpdateVarejo(long id, [FromBody] LancamentoVarejoDto dto)
        {
            var lancamento = await _context.LancamentosVarejo.FindAsync(id);
            if (lancamento == null) return NotFound();

            if (dto.DataLancamento.HasValue) lancamento.DataLancamento = dto.DataLancamento.Value;
            lancamento.QtdAtendimento = dto.QtdAtendimento;
            lancamento.QtdFechamento = dto.QtdFechamento;
            lancamento.Faturamento = dto.Faturamento;
            lancamento.QtdInstagram = dto.QtdInstagram;
            lancamento.QtdFacebook = dto.QtdFacebook;
            lancamento.QtdGoogle = dto.QtdGoogle;
            lancamento.QtdIndicacao = dto.QtdIndicacao;
            lancamento.IdClienteInvestimentoMeta = dto.IdClienteInvestimentoMeta;
            lancamento.IdClienteInvestimentoGoogle = dto.IdClienteInvestimentoGoogle;

            _context.LancamentosVarejo.Update(lancamento);
            await _context.SaveChangesAsync();
            return Ok(lancamento);
        }

        [HttpPut("cadastro/{id}")]
        public async Task<IActionResult> UpdateCadastro(long id, [FromBody] LancamentoCadastroDto dto)
        {
            var lancamento = await _context.LancamentosCadastro.FindAsync(id);
            if (lancamento == null) return NotFound();

            if (dto.DataLancamento.HasValue) lancamento.DataLancamento = dto.DataLancamento.Value;
            lancamento.QtdClickLink = dto.QtdClickLink;
            lancamento.QtdCadastros = dto.QtdCadastros;
            lancamento.VlrTicketMedio = dto.VlrTicketMedio;
            lancamento.IdClienteInvestimentoMeta = dto.IdClienteInvestimentoMeta;
            lancamento.IdClienteInvestimentoGoogle = dto.IdClienteInvestimentoGoogle;

            _context.LancamentosCadastro.Update(lancamento);
            await _context.SaveChangesAsync();
            return Ok(lancamento);
        }

        [HttpPut("saude/{id}")]
        public async Task<IActionResult> UpdateSaude(long id, [FromBody] LancamentoSaudeDto dto)
        {
            var lancamento = await _context.LancamentosSaude.FindAsync(id);
            if (lancamento == null) return NotFound();

            if (dto.DataLancamento.HasValue) lancamento.DataLancamento = dto.DataLancamento.Value;
            lancamento.QtdClickMeta = dto.QtdClickMeta;
            lancamento.QtdClickGoogle = dto.QtdClickGoogle;
            lancamento.QtdContatosReais = dto.QtdContatosReais;
            lancamento.QtdConversaoConsultas = dto.QtdConversaoConsultas;
            lancamento.VlrTicketMedioConsultas = dto.VlrTicketMedioConsultas;
            lancamento.QtdEntradaRedesSociais = dto.QtdEntradaRedesSociais;
            lancamento.QtdEntradaGoogle = dto.QtdEntradaGoogle;
            lancamento.IdClienteInvestimentoMeta = dto.IdClienteInvestimentoMeta;
            lancamento.IdClienteInvestimentoGoogle = dto.IdClienteInvestimentoGoogle;

            _context.LancamentosSaude.Update(lancamento);
            await _context.SaveChangesAsync();
            return Ok(lancamento);
        }

        // --- DELETE Endpoint ---
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var lancamento = await _context.Lancamentos.FindAsync(id);
            if (lancamento == null) return NotFound();

            // Optional: User can only delete their own
            // var userId = GetCurrentUserId();
            // if (lancamento.IdUsuario != userId && User.FindFirst("roleId")?.Value != "1") return Forbid();

            _context.Lancamentos.Remove(lancamento);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
