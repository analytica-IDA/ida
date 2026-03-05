namespace backend.Models
{
    public class UsuarioArea : BaseEntity
    {
        public long IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }

        public long IdArea { get; set; }
        public Area? Area { get; set; }
    }
}
