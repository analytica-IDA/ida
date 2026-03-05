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
        public ICollection<Pessoa> Pessoas { get; set; } = new List<Pessoa>();
        public ICollection<ClienteCargo> ClientesCargos { get; set; } = new List<ClienteCargo>();
        public ICollection<ClienteUsuario> ClientesUsuarios { get; set; } = new List<ClienteUsuario>();
    }
}
