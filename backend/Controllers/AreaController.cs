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
    public class AreaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AreaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] long? idCliente, [FromQuery] bool onlyVendedores = false)
        {
            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var claimClientId = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            if (roleId != 1) idCliente = claimClientId;

            var query = _context.Areas
                .Include(a => a.CargosAreas)
                    .ThenInclude(ca => ca.Cargo)
                        .ThenInclude(c => c!.ClientesCargos)
                            .ThenInclude(cc => cc.Cliente)
                .Include(a => a.CargosAreas)
                    .ThenInclude(ca => ca.Cargo)
                        .ThenInclude(c => c!.Role)
                .AsQueryable();

            if (idCliente.HasValue && idCliente > 0)
            {
                // Get area IDs linked to this client via Cargo -> ClienteCargo
                var areasFromCargo = _context.CargosAreas
                    .Where(ca => ca.Cargo!.ClientesCargos.Any(cc => cc.IdCliente == idCliente.Value));

                if (onlyVendedores)
                {
                    // Filter those areas to only include those where the cargo has a vendedor role
                    areasFromCargo = areasFromCargo.Where(ca => ca.Cargo!.IdRole == 4);
                }

                var areasFromCargoIds = await areasFromCargo.Select(ca => ca.IdArea).ToListAsync();

                // Get area IDs linked directly via ClienteUsuario
                var cuQuery = _context.ClientesUsuarios
                    .Where(cu => cu.IdCliente == idCliente.Value && cu.IdArea > 0);
                
                if (onlyVendedores)
                {
                    cuQuery = cuQuery.Where(cu => cu.Usuario!.Cargo!.IdRole == 4);
                }

                var areasFromCUIds = await cuQuery.Select(cu => cu.IdArea).ToListAsync();

                var allAreaIds = areasFromCargoIds.Concat(areasFromCUIds).Distinct().ToList();
                
                query = query.Where(a => allAreaIds.Contains(a.Id));
            }
            else if (roleId != 1) 
            {
                // Fallback for non-admin without explicit idCliente (should not happen with the idCliente = claimClientId assignment)
                query = query.Where(a => a.CargosAreas.Any(ca => ca.Cargo != null && ca.Cargo.ClientesCargos.Any(cc => cc.IdCliente == claimClientId)));
            }

            var dbAreas = await query.ToListAsync();
            
            var areas = dbAreas.Select(a => new 
            {
                a.Id,
                a.Nome,
                IdCargo = a.CargosAreas.FirstOrDefault()?.IdCargo,
                NomeCliente = a.CargosAreas.FirstOrDefault()?.Cargo?.ClientesCargos.FirstOrDefault()?.Cliente?.Nome
            }).ToList();

            return Ok(areas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var area = await _context.Areas
                .Include(a => a.CargosAreas)
                .ThenInclude(ca => ca.Cargo)
                .ThenInclude(c => c!.Role)
                .FirstOrDefaultAsync(a => a.Id == id);
            
            if (area == null) return NotFound();

            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idCliente = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            if (roleId != 1 && !area.CargosAreas.Any(ca => ca.Cargo != null && ca.Cargo.ClientesCargos.Any(cc => cc.IdCliente == idCliente))) 
                return Forbid();

            var result = new 
            {
                area.Id,
                area.Nome,
                IdCargo = area.CargosAreas.FirstOrDefault()?.IdCargo
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Area area)
        {
            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idCliente = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            if (roleId != 1 && area.IdCargo.HasValue)
            {
                var cargoValido = await _context.Cargos.AnyAsync(c => c.Id == area.IdCargo.Value && c.ClientesCargos.Any(cc => cc.IdCliente == idCliente));
                if (!cargoValido) return Forbid();
            }

            _context.Areas.Add(area);
            await _context.SaveChangesAsync();

            if (area.IdCargo.HasValue && area.IdCargo > 0)
            {
                _context.CargosAreas.Add(new CargoArea { IdCargo = area.IdCargo.Value, IdArea = area.Id });
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetById), new { id = area.Id }, area);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Area area)
        {
            if (id != area.Id) return BadRequest();

            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idCliente = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            var existingArea = await _context.Areas
                .Include(a => a.CargosAreas)
                .ThenInclude(ca => ca.Cargo)
                .ThenInclude(c => c!.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (existingArea == null) return NotFound();

            if (roleId != 1)
            {
                if (!existingArea.CargosAreas.Any(ca => ca.Cargo != null && ca.Cargo.ClientesCargos.Any(cc => cc.IdCliente == idCliente))) return Forbid();
                if (area.IdCargo.HasValue)
                {
                    var cargoValido = await _context.Cargos.AnyAsync(c => c.Id == area.IdCargo.Value && c.ClientesCargos.Any(cc => cc.IdCliente == idCliente));
                    if (!cargoValido) return Forbid();
                }
            }

            _context.Entry(area).State = EntityState.Modified;

            var existingLinks = _context.CargosAreas.Where(ca => ca.IdArea == id);
            _context.CargosAreas.RemoveRange(existingLinks);

            if (area.IdCargo.HasValue && area.IdCargo > 0)
            {
                _context.CargosAreas.Add(new CargoArea { IdCargo = area.IdCargo.Value, IdArea = id });
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var area = await _context.Areas
                .Include(a => a.CargosAreas)
                .ThenInclude(ca => ca.Cargo)
                .ThenInclude(c => c!.Role)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (area == null) return NotFound();

            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var idCliente = long.Parse(User.FindFirst("idCliente")?.Value ?? "0");

            if (roleId != 1 && !area.CargosAreas.Any(ca => ca.Cargo != null && ca.Cargo.ClientesCargos.Any(cc => cc.IdCliente == idCliente))) 
                return Forbid();

            _context.Areas.Remove(area);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
