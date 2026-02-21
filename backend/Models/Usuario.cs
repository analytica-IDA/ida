using System.Collections.Generic;

namespace backend.Models
{
    public class Usuario
    {
        // PK and FK with Pessoa
        public long Id { get; set; }
        public Pessoa? Pessoa { get; set; }

        public string Login { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public bool FlAtivo { get; set; } = true;
        public DateTime DtUltimaAtualizacao { get; set; } = DateTime.Now;

        public long IdCargo { get; set; }
        public Cargo? Cargo { get; set; }

        public long IdArea { get; set; }
        public Area? Area { get; set; }

        // Navigation property for many-to-many with Cliente
        public ICollection<ClienteUsuario> ClientesUsuarios { get; set; } = new List<ClienteUsuario>();
    }
}
