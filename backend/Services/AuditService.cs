using backend.Data;
using backend.Models;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IAuditService
    {
        Task LogAction(string usuario, string acao, string tabela, string dadosAntigos, string dadosNovos);
    }

    public class AuditService : IAuditService
    {
        private readonly AppDbContext _context;

        public AuditService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogAction(string usuario, string acao, string tabela, string dadosAntigos, string dadosNovos)
        {
            var log = new LogAuditoria
            {
                Usuario = usuario,
                Acao = acao,
                Tabela = tabela,
                DadosAntigos = dadosAntigos,
                DadosNovos = dadosNovos
            };

            await _context.LogsAuditoria.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
