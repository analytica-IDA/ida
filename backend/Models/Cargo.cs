using System.Collections.Generic;

namespace backend.Models
{
    public class Cargo : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        
        public long IdRole { get; set; }
        public Role? Role { get; set; }

        // Navigation properties
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}
