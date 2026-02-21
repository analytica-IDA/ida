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
            var cargos = await _context.Cargos.Include(c => c.Role).ToListAsync();
            return Ok(cargos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var cargo = await _context.Cargos.Include(c => c.Role).FirstOrDefaultAsync(c => c.Id == id);
            if (cargo == null) return NotFound();
            return Ok(cargo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cargo cargo)
        {
            _context.Cargos.Add(cargo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = cargo.Id }, cargo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Cargo cargo)
        {
            if (id != cargo.Id) return BadRequest();
            _context.Entry(cargo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var cargo = await _context.Cargos.FindAsync(id);
            if (cargo == null) return NotFound();
            _context.Cargos.Remove(cargo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
