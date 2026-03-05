namespace backend.Models
{
    public class RoleAplicacao : BaseEntity
    {
        public long IdRole { get; set; }
        public Role? Role { get; set; }

        public long IdAplicacao { get; set; }
        public Aplicacao? Aplicacao { get; set; }
    }
}
