using System.Collections.Generic;

namespace backend.Models
{
    public class Area : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;

        public long IdCliente { get; set; }
        public Cliente? Cliente { get; set; }

        // Navigation properties
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
