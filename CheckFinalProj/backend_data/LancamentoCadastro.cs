using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("lancamento_cadastro")]
    public class LancamentoCadastro : Lancamento
    {
        [Required]
        [Column("qtd_click_link")]
        public long QtdClickLink { get; set; }

        [Required]
        [Column("qtd_cadastros")]
        public long QtdCadastros { get; set; }

        [Required]
        [Column("vlr_ticket_medio", TypeName = "numeric(38,2)")]
        public decimal VlrTicketMedio { get; set; }

        [Required]
        [Column("vlr_investimento_meta", TypeName = "numeric(38,2)")]
        public decimal VlrInvestimentoMeta { get; set; } = 0;

        [Required]
        [Column("vlr_investimento_google", TypeName = "numeric(38,2)")]
        public decimal VlrInvestimentoGoogle { get; set; } = 0;
    }
}
