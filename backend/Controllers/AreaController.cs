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
    public class AreaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AreaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var areas = await _context.Areas.ToListAsync();
            return Ok(areas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area == null) return NotFound();
            return Ok(area);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Area area)
        {
            _context.Areas.Add(area);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = area.Id }, area);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] Area area)
        {
            if (id != area.Id) return BadRequest();
            _context.Entry(area).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var area = await _context.Areas.FindAsync(id);
            if (area == null) return NotFound();
            _context.Areas.Remove(area);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
