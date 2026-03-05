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

        // Navigation properties
        public ICollection<UsuarioArea> UsuariosAreas { get; set; } = new List<UsuarioArea>();
        public ICollection<ClienteUsuario> ClientesUsuarios { get; set; } = new List<ClienteUsuario>();
    }
}
