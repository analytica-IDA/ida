using backend.Data;
using backend.Models;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface INotificationService
    {
        Task CreateNotification(long usuarioId, string titulo, string mensagem, string tipo = "Sistema");
    }

    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateNotification(long usuarioId, string titulo, string mensagem, string tipo = "Sistema")
        {
            var notificacao = new Notificacao
            {
                IdUsuarioDestino = usuarioId,
                Titulo = titulo,
                Mensagem = mensagem,
                Tipo = tipo
            };

            await _context.Set<Notificacao>().AddAsync(notificacao);
            await _context.SaveChangesAsync();
        }
    }
}
