using System.Collections.Generic;

namespace backend.Models
{
    public class Cliente : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public byte[]? Foto { get; set; }

        // Navigation properties
        public ICollection<Area> Areas { get; set; } = new List<Area>();
        public ICollection<ClienteUsuario> ClientesUsuarios { get; set; } = new List<ClienteUsuario>();
    }
}
