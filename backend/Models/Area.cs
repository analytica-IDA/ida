using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Area : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;

        [NotMapped]
        public long? IdCargo { get; set; }

        // Navigation properties
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public ICollection<CargoArea> CargosAreas { get; set; } = new List<CargoArea>();
    }
}
