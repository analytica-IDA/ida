using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("lancamento_varejo")]
    public class LancamentoVarejo : Lancamento
    {
        [Required]
        [Column("qtd_atendimento")]
        public long QtdAtendimento { get; set; }

        [Required]
        [Column("qtd_fechamento")]
        public long QtdFechamento { get; set; }

        [Required]
        [Column("faturamento", TypeName = "numeric(38,2)")]
        public decimal Faturamento { get; set; }

        [Required]
        [Column("qtd_instagram")]
        public long QtdInstagram { get; set; }

        [Required]
        [Column("qtd_facebook")]
        public long QtdFacebook { get; set; }

        [Required]
        [Column("qtd_google")]
        public long QtdGoogle { get; set; }

        [Required]
        [Column("qtd_indicacao")]
        public long QtdIndicacao { get; set; }

        [Required]
        [Column("vlr_investimento_meta", TypeName = "numeric(38,2)")]
        public decimal VlrInvestimentoMeta { get; set; } = 0;

        [Required]
        [Column("vlr_investimento_google", TypeName = "numeric(38,2)")]
        public decimal VlrInvestimentoGoogle { get; set; } = 0;
    }
}
