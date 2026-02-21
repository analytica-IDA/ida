using System;

namespace backend.Models
{
    public class Pessoa : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public byte[]? Foto { get; set; }
        public DateTime? DataNascimento { get; set; }

        public long? IdCliente { get; set; }

        // Navigation property
        public Cliente? Cliente { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
