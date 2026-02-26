using backend.Data;
using backend.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

            // Notification Logic
            var originUser = await _context.Usuarios
                .Include(u => u.Cargo).ThenInclude(c => c!.Role)
                .Include(u => u.Pessoa)
                .FirstOrDefaultAsync(u => u.Login == usuario);

            if (originUser?.Cargo?.Role?.Nome?.ToLower() == "vendedor")
            {
                var idCliente = originUser.Pessoa?.IdCliente;
                
                var targetUsers = await _context.Usuarios
                    .Include(u => u.Cargo).ThenInclude(c => c!.Role)
                    .Include(u => u.Pessoa)
                    .Where(u => u.FlAtivo && u.Cargo != null && u.Cargo.Role != null && (
                        u.Cargo.Role.Nome.ToLower() == "admin" || 
                        ((u.Cargo.Role.Nome.ToLower() == "proprietário" || u.Cargo.Role.Nome.ToLower() == "supervisor") && u.Pessoa != null && u.Pessoa.IdCliente == idCliente)
                    ))
                    .ToListAsync();

                var nomeVendedor = originUser.Pessoa?.Nome ?? usuario;
                var acaoFormatada = acao.ToUpper() switch
                {
                    "CREATE" => "criou um novo registro",
                    "UPDATE" => "atualizou um registro",
                    "DELETE" => "excluiu um registro",
                    _ => $"realizou a ação '{acao}'"
                };

                foreach (var target in targetUsers)
                {
                    // Avoid notifying oneself if for some reason a vendedor acts as supervisor?
                    if (target.Id == originUser.Id) continue; 

                    var notificacao = new Notificacao
                    {
                        IdUsuarioDestino = target.Id,
                        Titulo = $"Atividade de {nomeVendedor}",
                        Mensagem = $"O vendedor {nomeVendedor} {acaoFormatada} em '{tabela}'.",
                        Tipo = "AtividadeVendedor"
                    };
                    await _context.Notificacoes.AddAsync(notificacao);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
