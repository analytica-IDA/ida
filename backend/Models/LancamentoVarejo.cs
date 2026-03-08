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

        [Column("id_cliente_investimento_meta")]
        public long? IdClienteInvestimentoMeta { get; set; }

        [ForeignKey("IdClienteInvestimentoMeta")]
        public ClienteInvestimentoMeta? ClienteInvestimentoMeta { get; set; }

        [Column("id_cliente_investimento_google")]
        public long? IdClienteInvestimentoGoogle { get; set; }

        [ForeignKey("IdClienteInvestimentoGoogle")]
        public ClienteInvestimentoGoogle? ClienteInvestimentoGoogle { get; set; }
    }
}
