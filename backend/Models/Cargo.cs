using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Cargo : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        
        public long IdRole { get; set; }
        public Role? Role { get; set; }

        [NotMapped]
        public long? IdCliente { get; set; }

        // Navigation properties
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public ICollection<ClienteCargo> ClientesCargos { get; set; } = new List<ClienteCargo>();
        public ICollection<CargoArea> CargosAreas { get; set; } = new List<CargoArea>();
    }
}
