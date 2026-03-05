using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebugController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly backend.Services.IAuthService _authService;

        public DebugController(AppDbContext context, backend.Services.IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpGet("user/{login}")]
        public async Task<IActionResult> InspectUser(string login)
        {
            var userRaw = await _context.Usuarios.FirstOrDefaultAsync(u => u.Login == login);
            var userJoined = await _context.Usuarios
                .Include(u => u.Pessoa)
                .Include(u => u.Cargo)
                .ThenInclude(c => c!.Role)
                .FirstOrDefaultAsync(u => u.Login == login);

            if (userRaw == null)
                return Ok(new { error = "User not found in raw query" });

            return Ok(new { 
                rawExists = true, 
                joinedExists = userJoined != null,
                id = userRaw.Id,
                hash = userRaw.Senha,
                idCargo = userRaw.IdCargo,
                pessoaExists = await _context.Pessoas.AnyAsync(p => p.Id == userRaw.Id),
                cargoExists = await _context.Cargos.AnyAsync(c => c.Id == userRaw.IdCargo),
                roleExists = await _context.Cargos.Where(c => c.Id == userRaw.IdCargo).Select(c => _context.Roles.Any(r => r.Id == c.IdRole)).FirstOrDefaultAsync()
            });
        }

        [HttpGet("test-hash")]
        public async Task<IActionResult> TestHash([FromQuery] string login, [FromQuery] string senha)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Login == login);
            if (user == null) return Ok(new { error = "User not found" });
            
            bool valid = _authService.ValidatePassword(senha, user.Senha);
            return Ok(new { 
                inputSenha = senha, 
                inputLen = senha?.Length,
                dbSenha = user.Senha, 
                dbLen = user.Senha?.Length,
                valid = valid
            });
        }
    }
}
