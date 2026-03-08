using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("cliente_investimento_google")]
    public class ClienteInvestimentoGoogle : BaseEntity
    {
        [Required]
        [Column("id_cliente")]
        public long IdCliente { get; set; }

        [ForeignKey("IdCliente")]
        public Cliente? Cliente { get; set; }

        [Required]
        [Column("vlr_investimento_google", TypeName = "decimal(38,2)")]
        public decimal VlrInvestimentoGoogle { get; set; } = 0;
    }
}
