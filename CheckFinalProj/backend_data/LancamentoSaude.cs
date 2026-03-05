using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("lancamento_saude")]
    public class LancamentoSaude : Lancamento
    {
        [Required]
        [Column("qtd_click_meta")]
        public long QtdClickMeta { get; set; }

        [Required]
        [Column("qtd_click_google")]
        public long QtdClickGoogle { get; set; }

        [Required]
        [Column("qtd_contatos_reais")]
        public long QtdContatosReais { get; set; }

        [Required]
        [Column("qtd_conversao_consultas")]
        public long QtdConversaoConsultas { get; set; }

        [Column("vlr_ticket_medio_consultas", TypeName = "numeric(38,2)")]
        public decimal? VlrTicketMedioConsultas { get; set; }

        [Required]
        [Column("qtd_entrada_redes_sociais")]
        public long QtdEntradaRedesSociais { get; set; }

        [Required]
        [Column("qtd_entrada_google")]
        public long QtdEntradaGoogle { get; set; }

        [Column("vlr_investimento_meta", TypeName = "numeric(38,12)")]
        public decimal? VlrInvestimentoMeta { get; set; }

        [Column("vlr_investimento_google", TypeName = "numeric(38,2)")]
        public decimal? VlrInvestimentoGoogle { get; set; }
    }
}
