namespace backend.Models
{
    public class ClienteModeloControle : BaseEntity
    {
        public long IdCliente { get; set; }
        public Cliente? Cliente { get; set; }

        public long IdModeloControle { get; set; }
        public ModeloControle? ModeloControle { get; set; }
    }
}
