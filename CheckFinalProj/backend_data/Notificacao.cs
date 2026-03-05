namespace backend.Models
{
    public class Notificacao : BaseEntity
    {
        public string Titulo { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
        public bool Lida { get; set; } = false;
        public long IdUsuarioDestino { get; set; }
        public Usuario? UsuarioDestino { get; set; }
        public string Tipo { get; set; } = "Sistema"; // "AtividadeVendedor", "Alerta", etc.
    }
}
