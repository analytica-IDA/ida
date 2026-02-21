using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;
        private readonly IAuditService _auditService;
        private readonly INotificationService _notificationService;
        private readonly IBackupService _backupService;

        public UserController(AppDbContext context, IAuthService authService, IAuditService auditService, INotificationService notificationService, IBackupService backupService)
        {
            _context = context;
            _authService = authService;
            _auditService = auditService;
            _notificationService = notificationService;
            _backupService = backupService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Usuarios
                .Include(u => u.Cargo)
                .ThenInclude(c => c!.Role)
                .FirstOrDefaultAsync(u => u.Login == request.Login);

            if (user == null || !_authService.ValidatePassword(request.Senha, user.Senha))
            {
                return Unauthorized(new { message = "Login ou senha inválidos" });
            }

            var token = _authService.GenerateToken(user);
            
            await _auditService.LogAction(user.Login, "LOGIN", "usuario", "", "Login realizado com sucesso");

            return Ok(new { token, user = new { user.Login, role = user.Cargo?.Role?.Nome } });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Usuarios
                .Include(u => u.Pessoa)
                .Include(u => u.Cargo)
                .ThenInclude(c => c!.Role)
                .Include(u => u.Area)
                .Select(u => new 
                {
                    u.Id,
                    Nome = u.Pessoa!.Nome,
                    u.Login,
                    Email = u.Pessoa!.Email,
                    Cargo = u.Cargo!.Nome,
                    RoleDescription = u.Cargo!.Role!.Nome,
                    Area = u.Area!.Nome,
                    u.FlAtivo,
                    u.DtUltimaAtualizacao
                })
                .ToListAsync();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }

        [Authorize]
        [HttpGet("cargos")]
        public async Task<IActionResult> GetCargos()
        {
            var cargos = await _context.Cargos.ToListAsync();
            return Ok(cargos);
        }

        [Authorize]
        [HttpGet("areas")]
        public async Task<IActionResult> GetAreas()
        {
            var areas = await _context.Areas.ToListAsync();
            return Ok(areas);
        }

        [Authorize]
        [HttpGet("menu")]
        public async Task<IActionResult> GetMenu()
        {
            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            
            var apps = await _context.RolesAplicacoes
                .Where(ra => ra.IdRole == roleId)
                .Include(ra => ra.Aplicacao)
                .Select(ra => ra.Aplicacao!.Nome)
                .ToListAsync();

            return Ok(apps);
        }

        [Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var currentUserId = long.Parse(User.FindFirst("id")?.Value ?? "0");
            var currentUser = await _context.Usuarios
                .Include(u => u.Cargo)
                .ThenInclude(c => c!.Role)
                .FirstOrDefaultAsync(u => u.Id == currentUserId);

            if (currentUser == null) return Unauthorized();

            var currentUserRole = currentUser.Cargo?.Role?.Nome;
            
            // For simplicity, we are getting a CargoId directly from frontend in this refined version
            var targetCargo = await _context.Cargos
                .Include(c => c.Role)
                .FirstOrDefaultAsync(c => c.Id == request.IdCargo);

            if (targetCargo == null) return BadRequest("Cargo alvo inválido");

            var targetRoleName = targetCargo.Role?.Nome;

            // Role constraints
            bool canCreate = false;
            if (currentUserRole == "admin") canCreate = true;
            else if (currentUserRole == "proprietário" && (targetRoleName == "supervisor" || targetRoleName == "vendedor")) canCreate = true;
            else if (currentUserRole == "supervisor" && targetRoleName == "vendedor") canCreate = true;

            if (!canCreate) return Forbid();

            // Check if login already exists
            if (await _context.Usuarios.AnyAsync(u => u.Login == request.Login))
            {
                return BadRequest(new { message = "Este login já está em uso" });
            }

            // Check if Pessoa exists
            var pessoaExists = await _context.Pessoas.AnyAsync(p => p.Id == request.IdPessoa);
            if (!pessoaExists) return NotFound(new { message = "Pessoa não encontrada" });

            // Check if Usuario already exists for this Pessoa
            var userExists = await _context.Usuarios.AnyAsync(u => u.Id == request.IdPessoa);
            if (userExists) return BadRequest(new { message = "Esta pessoa já possui um usuário vinculado" });

            try
            {
                var usuario = new Usuario
                {
                    Id = request.IdPessoa,
                    Login = request.Login,
                    Senha = _authService.HashPassword(request.Senha),
                    IdCargo = request.IdCargo,
                    IdArea = request.IdArea,
                    FlAtivo = true,
                    DtUltimaAtualizacao = DateTime.Now
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                await _auditService.LogAction(currentUser.Login, "REGISTER_USER", "usuario", usuario.Id.ToString(), $"Usuário {request.Login} criado e vinculado à pessoa {request.IdPessoa}");

                return Ok(new { message = "Usuário registrado com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao criar usuário", error = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Login == request.Login);
            if (user == null) return NotFound("Usuário não encontrado");

            // Mock email sending
            await _auditService.LogAction(request.Login, "FORGOT_PASSWORD", "usuario", "", "Recuperação de senha solicitada");
            return Ok(new { message = "Instruções de recuperação enviadas para o email cadastrado" });
        }

        [Authorize]
        [HttpPost("backup")]
        public IActionResult Backup()
        {
            var roleId = long.Parse(User.FindFirst("roleId")?.Value ?? "0");
            var role = _context.Roles.Find(roleId);
            if (role?.Nome != "admin") return Forbid("Apenas administradores podem realizar backup");

            var path = _backupService.CreateBackup();
            return Ok(new { message = "Backup concluído", path });
        }
    }

    public class LoginRequest { public string Login { get; set; } = ""; public string Senha { get; set; } = ""; }
    public class RegisterRequest 
    { 
        public long IdPessoa { get; set; }
        public string Login { get; set; } = ""; 
        public string Senha { get; set; } = ""; 
        public long IdCargo { get; set; } 
        public long IdArea { get; set; }
    }
    public class ForgotPasswordRequest { public string Login { get; set; } = ""; }
}
