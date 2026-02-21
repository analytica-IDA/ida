using System;

namespace backend.Models
{
    public class LogAuditoria : BaseEntity
    {
        public string Usuario { get; set; } = string.Empty;
        public string Acao { get; set; } = string.Empty;
        public string Tabela { get; set; } = string.Empty;
        public string DadosAntigos { get; set; } = string.Empty;
        public string DadosNovos { get; set; } = string.Empty;
        public DateTime DataHora { get; set; } = DateTime.Now;
    }
}
