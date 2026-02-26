using backend.Data;
using backend.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace backend.Services
{
    public interface INotificationService
    {
        Task CreateNotification(long usuarioId, string titulo, string Mensagem, string tipo = "Sistema");
        Task<List<Notificacao>> GetNotificacoesNaoLidasAsync(long usuarioId);
        Task MarcarComoLidaAsync(long notificacaoId, long usuarioId);
    }

    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateNotification(long usuarioId, string titulo, string Mensagem, string tipo = "Sistema")
        {
            var notificacao = new Notificacao
            {
                IdUsuarioDestino = usuarioId,
                Titulo = titulo,
                Mensagem = Mensagem,
                Tipo = tipo
            };

            await _context.Set<Notificacao>().AddAsync(notificacao);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Notificacao>> GetNotificacoesNaoLidasAsync(long usuarioId)
        {
            return await _context.Set<Notificacao>()
                .Where(n => n.IdUsuarioDestino == usuarioId && !n.Lida)
                .OrderByDescending(n => n.DtUltimaAtualizacao)
                .ToListAsync();
        }

        public async Task MarcarComoLidaAsync(long notificacaoId, long usuarioId)
        {
            var notificacao = await _context.Set<Notificacao>()
                .FirstOrDefaultAsync(n => n.Id == notificacaoId && n.IdUsuarioDestino == usuarioId);
            
            if (notificacao != null)
            {
                notificacao.Lida = true;
                notificacao.DtUltimaAtualizacao = System.DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}
