using System;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class LancamentoVarejoDto
    {
        public long? Id { get; set; }
        [Required] public long IdCliente { get; set; }
        public DateTime? DataLancamento { get; set; }
        [Required] public long QtdAtendimento { get; set; }
        [Required] public long QtdFechamento { get; set; }
        [Required] public decimal Faturamento { get; set; }
        [Required] public long QtdInstagram { get; set; }
        [Required] public long QtdFacebook { get; set; }
        [Required] public long QtdGoogle { get; set; }
        [Required] public long QtdIndicacao { get; set; }
        public decimal VlrInvestimentoMeta { get; set; } = 0;
        public decimal VlrInvestimentoGoogle { get; set; } = 0;
    }

    public class LancamentoCadastroDto
    {
        public long? Id { get; set; }
        [Required] public long IdCliente { get; set; }
        public DateTime? DataLancamento { get; set; }
        [Required] public long QtdClickLink { get; set; }
        [Required] public long QtdCadastros { get; set; }
        [Required] public decimal VlrTicketMedio { get; set; }
        public decimal VlrInvestimentoMeta { get; set; } = 0;
        public decimal VlrInvestimentoGoogle { get; set; } = 0;
    }

    public class LancamentoSaudeDto
    {
        public long? Id { get; set; }
        [Required] public long IdCliente { get; set; }
        public DateTime? DataLancamento { get; set; }
        [Required] public long QtdClickMeta { get; set; }
        [Required] public long QtdClickGoogle { get; set; }
        [Required] public long QtdContatosReais { get; set; }
        [Required] public long QtdConversaoConsultas { get; set; }
        public decimal? VlrTicketMedioConsultas { get; set; }
        [Required] public long QtdEntradaRedesSociais { get; set; }
        [Required] public long QtdEntradaGoogle { get; set; }
        public decimal? VlrInvestimentoMeta { get; set; }
        public decimal? VlrInvestimentoGoogle { get; set; }
    }
}
