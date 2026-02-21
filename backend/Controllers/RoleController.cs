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
    public class RoleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChanges();
            return Ok(role);
        }
    }
}
