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
        public async Task<IActionResult> GetByCliente(long idCliente, [FromQuery] long? idModeloControle, [FromQuery] DateTime? dataInicial, [FromQuery] DateTime? dataFinal)
        {
            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            bool isAdmin = (roleId == 1); // Role 1 is Admin in the DB seed

            // Verify if client has this modelo
            var hasModel = await _context.ClientesModelosControles
                .AnyAsync(cm => cm.IdCliente == idCliente && cm.IdModeloControle == idModeloControle);

            if (!hasModel) return BadRequest(new { message = "Cliente não possui o modelo de controle informado." });

            // Base query for users
            List<long> userIds = new List<long>();
            
            if (isAdmin)
            {
                // Admin sees ALL users linked to this specific client
                userIds = await _context.ClientesUsuarios
                    .Where(cu => cu.IdCliente == idCliente)
                    .Select(cu => cu.IdUsuario)
                    .Distinct()
                    .ToListAsync();
            }
            else
            {
                // Normal user sees users filtered conceptually by hierarchy. For now, matching existing logic (users on this client)
                userIds = await _context.ClientesUsuarios
                    .Where(cu => cu.IdCliente == idCliente)
                    .Select(cu => cu.IdUsuario)
                    .Distinct()
                    .ToListAsync();
            }

            // We need to return specific type based on modelo
            if (idModeloControle == 1) // Cadastro
            {
                var query = _context.LancamentosCadastro
                    .Where(l => userIds.Contains(l.IdUsuario))
                    .Include(l => l.Usuario).ThenInclude(u => u.Pessoa)
                    .AsQueryable();
                    
                if (dataInicial.HasValue) query = query.Where(l => l.DataLancamento >= dataInicial.Value);
                if (dataFinal.HasValue) query = query.Where(l => l.DataLancamento <= dataFinal.Value.AddDays(1).AddTicks(-1));
                
                return Ok(await query.OrderByDescending(l => l.DataLancamento).ToListAsync());
            }
            else if (idModeloControle == 2) // Varejo
            {
                var query = _context.LancamentosVarejo
                    .Where(l => userIds.Contains(l.IdUsuario))
                    .Include(l => l.Usuario).ThenInclude(u => u.Pessoa)
                    .AsQueryable();
                    
                if (dataInicial.HasValue) query = query.Where(l => l.DataLancamento >= dataInicial.Value);
                if (dataFinal.HasValue) query = query.Where(l => l.DataLancamento <= dataFinal.Value.AddDays(1).AddTicks(-1));
                
                return Ok(await query.OrderByDescending(l => l.DataLancamento).ToListAsync());
            }
            else if (idModeloControle == 3) // Saude
            {
                var query = _context.LancamentosSaude
                    .Where(l => userIds.Contains(l.IdUsuario))
                    .Include(l => l.Usuario).ThenInclude(u => u.Pessoa)
                    .AsQueryable();
                    
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
            lancamento.VlrInvestimentoMeta = dto.VlrInvestimentoMeta;
            lancamento.VlrInvestimentoGoogle = dto.VlrInvestimentoGoogle;

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
            lancamento.VlrInvestimentoMeta = dto.VlrInvestimentoMeta;
            lancamento.VlrInvestimentoGoogle = dto.VlrInvestimentoGoogle;

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
            lancamento.VlrInvestimentoMeta = dto.VlrInvestimentoMeta;
            lancamento.VlrInvestimentoGoogle = dto.VlrInvestimentoGoogle;

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
