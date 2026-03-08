using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteInvestimentoMetaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteInvestimentoMetaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idCliente = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            var query = _context.ClientesInvestimentosMeta.AsQueryable();

            if (roleId != 1) // If not admin
            {
                query = query.Where(c => c.IdCliente == idCliente);
            }

            return Ok(await query.OrderByDescending(c => c.DtUltimaAtualizacao).ToListAsync());
        }

        [HttpGet("cliente/{idCliente}")]
        public async Task<IActionResult> GetByCliente(long idCliente)
        {
            var invest = await _context.ClientesInvestimentosMeta
                .Where(c => c.IdCliente == idCliente)
                .OrderByDescending(c => c.DtUltimaAtualizacao)
                .FirstOrDefaultAsync();

            return Ok(invest);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClienteInvestimentoMeta invest)
        {
            _context.ClientesInvestimentosMeta.Add(invest);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByCliente), new { idCliente = invest.IdCliente }, invest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] ClienteInvestimentoMeta invest)
        {
            if (id != invest.Id) return BadRequest();
            _context.Entry(invest).State = EntityState.Modified;
            invest.DtUltimaAtualizacao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var invest = await _context.ClientesInvestimentosMeta.FindAsync(id);
            if (invest == null) return NotFound();
            _context.ClientesInvestimentosMeta.Remove(invest);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
