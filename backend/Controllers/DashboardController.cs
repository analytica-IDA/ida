using Microsoft.AspNetCore.Mvc;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var activeUsers = await _context.Usuarios.CountAsync(u => u.FlAtivo);
            var partnerClients = await _context.Clientes.CountAsync();
            var registeredPersons = await _context.Pessoas.CountAsync();
            var reportsGenerated = await _context.LogsAuditoria.CountAsync();

            return Ok(new
            {
                activeUsers,
                partnerClients,
                registeredPersons,
                reportsGenerated
            });
        }
    }
}
