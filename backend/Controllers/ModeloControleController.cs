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
    public class ModeloControleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ModeloControleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var modelos = await _context.ModelosControles.ToListAsync();
            return Ok(modelos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var modelo = await _context.ModelosControles.FindAsync(id);
            if (modelo == null) return NotFound();
            return Ok(modelo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ModeloControle modelo)
        {
            _context.ModelosControles.Add(modelo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = modelo.Id }, modelo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] ModeloControle modelo)
        {
            if (id != modelo.Id) return BadRequest();

            _context.Entry(modelo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.ModelosControles.AnyAsync(m => m.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var modelo = await _context.ModelosControles.FindAsync(id);
            if (modelo == null) return NotFound();

            _context.ModelosControles.Remove(modelo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
