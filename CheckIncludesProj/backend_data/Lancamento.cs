using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("lancamento")]
    public class Lancamento : BaseEntity
    {
        [Required]
        [Column("id_usuario")]
        public long IdUsuario { get; set; }
        
        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }

        [Required]
        [Column("id_modelo_controle")]
        public long IdModeloControle { get; set; }

        [ForeignKey("IdModeloControle")]
        public ModeloControle? ModeloControle { get; set; }

        [Required]
        [Column("data_lancamento")]
        public DateTime DataLancamento { get; set; } = DateTime.UtcNow;
    }
}
