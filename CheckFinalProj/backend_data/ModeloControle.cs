using System.Collections.Generic;

namespace backend.Models
{
    public class ModeloControle : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<ClienteModeloControle> ClientesModelosControles { get; set; } = new List<ClienteModeloControle>();
    }
}
