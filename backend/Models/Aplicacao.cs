using System.Collections.Generic;

namespace backend.Models
{
    public class Aplicacao : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<RoleAplicacao> RolesAplicacoes { get; set; } = new List<RoleAplicacao>();
    }
}
