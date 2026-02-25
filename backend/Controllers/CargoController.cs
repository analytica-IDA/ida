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
    public class CargoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CargoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idCliente = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            var query = _context.Cargos
                .Include(c => c.Role)
                .Include(c => c.ClientesCargos)
                    .ThenInclude(cc => cc.Cliente)
                .AsQueryable();

            // Hierarquia: 
            // 1 = Admin (vê tudo do sistema)
            // 2 = Proprietário (vê seu tenant, cargos de supervisor e vendedor)
            // 3 = Supervisor (vê seu tenant, apenas cargos de vendedor)
            // 4 = Vendedor (não tem acesso a essa tela de cargo, mas se listar, não vê nada)
            
            if (roleId != 1) 
            {
                query = query.Where(c => c.ClientesCargos.Any(cc => cc.IdCliente == idCliente));
                
                if (roleId == 2) // Proprietário
                {
                    query = query.Where(c => c.Role!.Nome == "supervisor" || c.Role!.Nome == "vendedor" || c.Role!.Nome == "Supervisor" || c.Role!.Nome == "Vendedor");
                }
                else if (roleId == 3) // Supervisor
                {
                    query = query.Where(c => c.Role!.Nome == "vendedor" || c.Role!.Nome == "Vendedor");
                }
                else 
                {
                    query = query.Where(c => false); // Outras roles não veem cargos
                }
            }

            var cargos = await query
                .Select(c => new 
                {
                    c.Id,
                    c.Nome,
                    c.IdRole,
                    Role = c.Role,
                    IdCliente = c.ClientesCargos.FirstOrDefault() != null ? (long?)c.ClientesCargos.FirstOrDefault()!.IdCliente : null,
                    NomeCliente = c.ClientesCargos.FirstOrDefault() != null ? c.ClientesCargos.FirstOrDefault()!.Cliente!.Nome : null
                })
                .ToListAsync();

            return Ok(cargos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var cargo = await _context.Cargos
                .Include(c => c.Role)
                .Include(c => c.ClientesCargos)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (cargo == null) return NotFound();

            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idCliente = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            if (roleId != 1 && !cargo.ClientesCargos.Any(cc => cc.IdCliente == idCliente)) 
                return Forbid();

            var result = new 
            {
                cargo.Id,
                cargo.Nome,
                cargo.IdRole,
                Role = cargo.Role,
                IdCliente = cargo.ClientesCargos.FirstOrDefault()?.IdCliente
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cargo cargo)
        {
            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idCliente = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            if (roleId != 1) 
            {
                cargo.IdCliente = idCliente; // Force tenant
            }

            _context.Cargos.Add(cargo);
            await _context.SaveChangesAsync();

            if (cargo.IdCliente.HasValue && cargo.IdCliente > 0)
            {
                _context.ClientesCargos.Add(new ClienteCargo { IdCliente = cargo.IdCliente.Value, IdCargo = cargo.Id });
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetById), new { id = cargo.Id }, cargo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Cargo cargo)
        {
            if (id != cargo.Id) return BadRequest();

            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idCliente = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            var existingCargo = await _context.Cargos
                .Include(c => c.ClientesCargos)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingCargo == null) return NotFound();

            if (roleId != 1) 
            {
                if (!existingCargo.ClientesCargos.Any(cc => cc.IdCliente == idCliente)) return Forbid();
                cargo.IdCliente = idCliente; // Force tenant
            }

            _context.Entry(cargo).State = EntityState.Modified;

            var existingLinks = _context.ClientesCargos.Where(cc => cc.IdCargo == id);
            _context.ClientesCargos.RemoveRange(existingLinks);

            if (cargo.IdCliente.HasValue && cargo.IdCliente > 0)
            {
                _context.ClientesCargos.Add(new ClienteCargo { IdCliente = cargo.IdCliente.Value, IdCargo = id });
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var cargo = await _context.Cargos
                .Include(c => c.ClientesCargos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cargo == null) return NotFound();

            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idCliente = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            if (roleId != 1 && !cargo.ClientesCargos.Any(cc => cc.IdCliente == idCliente)) 
                return Forbid();

            _context.Cargos.Remove(cargo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
