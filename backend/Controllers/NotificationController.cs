using Microsoft.AspNetCore.Mvc;
using backend.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Authorize]
        [HttpGet("nao-lidas")]
        public async Task<IActionResult> GetNotificacoesNaoLidas()
        {
            var userId = long.Parse(User.FindFirst("id")?.Value ?? "0");
            if (userId == 0) return Unauthorized();

            var notificacoes = await _notificationService.GetNotificacoesNaoLidasAsync(userId);
            return Ok(notificacoes);
        }

        [Authorize]
        [HttpPost("{id}/ler")]
        public async Task<IActionResult> MarcarComoLida(long id)
        {
            var userId = long.Parse(User.FindFirst("id")?.Value ?? "0");
            if (userId == 0) return Unauthorized();

            await _notificationService.MarcarComoLidaAsync(id, userId);
            return Ok(new { message = "Notificação marcada como lida." });
        }
    }
}
