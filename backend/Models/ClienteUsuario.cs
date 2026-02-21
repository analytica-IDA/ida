namespace backend.Models
{
    public class ClienteUsuario : BaseEntity
    {
        public long IdCliente { get; set; }
        public Cliente? Cliente { get; set; }

        public long IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
