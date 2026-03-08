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
