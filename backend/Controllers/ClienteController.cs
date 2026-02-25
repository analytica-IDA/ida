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
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cliente cliente)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                // Check if current user is admin (roleId == 1)
                var roleIdClaim = User.FindFirst("roleId")?.Value;
                if (roleIdClaim == "1")
                {
                    // 1. Create default Cargo "Proprietário(a)" (Role ID 2)
                    var cargoProp = new Cargo { Nome = "Proprietário(a)", IdRole = 2 };
                    _context.Cargos.Add(cargoProp);
                    await _context.SaveChangesAsync();

                    _context.ClientesCargos.Add(new ClienteCargo { IdCliente = cliente.Id, IdCargo = cargoProp.Id });

                    var areaProp = new Area { Nome = "Proprietário(a)" };
                    _context.Areas.Add(areaProp);
                    await _context.SaveChangesAsync();

                    _context.CargosAreas.Add(new CargoArea { IdCargo = cargoProp.Id, IdArea = areaProp.Id });

                    // 2. Create default Cargo "Vendedor" (Role ID 4)
                    var cargoVend = new Cargo { Nome = "Vendedor", IdRole = 4 };
                    _context.Cargos.Add(cargoVend);
                    await _context.SaveChangesAsync();

                    _context.ClientesCargos.Add(new ClienteCargo { IdCliente = cliente.Id, IdCargo = cargoVend.Id });

                    var areaVend = new Area { Nome = "Vendedor" };
                    _context.Areas.Add(areaVend);
                    await _context.SaveChangesAsync();

                    _context.CargosAreas.Add(new CargoArea { IdCargo = cargoVend.Id, IdArea = areaVend.Id });

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id) return BadRequest();
            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/usuarios")]
        public async Task<IActionResult> GetUsuarios(long id)
        {
            var associations = await _context.ClientesUsuarios
                .Where(cu => cu.IdCliente == id)
                .Include(cu => cu.Usuario)
                    .ThenInclude(u => u!.Pessoa)
                .Include(cu => cu.Area)
                .Select(cu => new {
                    cu.IdUsuario,
                    Nome = cu.Usuario!.Pessoa!.Nome,
                    Area = cu.Area!.Nome,
                    cu.IdArea
                })
                .ToListAsync();

            return Ok(associations);
        }

        [HttpPost("{id}/usuarios")]
        public async Task<IActionResult> AssociateUsuario(long id, [FromBody] AssociateRequest request)
        {
            var exists = await _context.ClientesUsuarios.AnyAsync(cu => cu.IdCliente == id && cu.IdUsuario == request.IdUsuario);
            if (exists) return BadRequest(new { message = "Usuário já vinculado a este cliente" });

            var association = new ClienteUsuario
            {
                IdCliente = id,
                IdUsuario = request.IdUsuario,
                IdArea = request.IdArea
            };

            _context.ClientesUsuarios.Add(association);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário vinculado com sucesso" });
        }

        [HttpDelete("{id}/usuarios/{userId}")]
        public async Task<IActionResult> RemoveUsuario(long id, long userId)
        {
            var association = await _context.ClientesUsuarios.FirstOrDefaultAsync(cu => cu.IdCliente == id && cu.IdUsuario == userId);
            if (association == null) return NotFound();

            _context.ClientesUsuarios.Remove(association);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

    public class AssociateRequest
    {
        public long IdUsuario { get; set; }
        public long IdArea { get; set; }
    }
}
