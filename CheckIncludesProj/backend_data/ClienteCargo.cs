using System.Collections.Generic;

namespace backend.Models
{
    public class ClienteCargo : BaseEntity
    {
        public long IdCliente { get; set; }
        public Cliente? Cliente { get; set; }

        public long IdCargo { get; set; }
        public Cargo? Cargo { get; set; }
    }
}
