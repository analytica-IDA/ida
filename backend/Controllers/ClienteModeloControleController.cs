using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteModeloControleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteModeloControleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idClienteToken = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            var query = _context.ClientesModelosControles
                .Include(cm => cm.Cliente)
                .Include(cm => cm.ModeloControle)
                .AsQueryable();

            if (roleId != 1) 
            {
                query = query.Where(cm => cm.IdCliente == idClienteToken);
            }

            var result = await query.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var connection = await _context.ClientesModelosControles
                .Include(cm => cm.Cliente)
                .Include(cm => cm.ModeloControle)
                .FirstOrDefaultAsync(cm => cm.Id == id);

            if (connection == null) return NotFound();

            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idClienteToken = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            if (roleId != 1 && connection.IdCliente != idClienteToken) 
                return Forbid();

            return Ok(connection);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClienteModeloControle connection)
        {
            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idClienteToken = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            if (roleId != 1 && connection.IdCliente != idClienteToken) 
                return Forbid();

            _context.ClientesModelosControles.Add(connection);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = connection.Id }, connection);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] ClienteModeloControle connection)
        {
            if (id != connection.Id) return BadRequest();

            var existing = await _context.ClientesModelosControles.AsNoTracking().FirstOrDefaultAsync(cm => cm.Id == id);
            if (existing == null) return NotFound();

            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idClienteToken = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            if (roleId != 1 && existing.IdCliente != idClienteToken) 
                return Forbid();
            if (roleId != 1 && connection.IdCliente != idClienteToken) 
                return Forbid();

            _context.Entry(connection).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var connection = await _context.ClientesModelosControles.FindAsync(id);
            if (connection == null) return NotFound();

            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idClienteToken = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            if (roleId != 1 && connection.IdCliente != idClienteToken) 
                return Forbid();

            _context.ClientesModelosControles.Remove(connection);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
