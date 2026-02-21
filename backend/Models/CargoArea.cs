using System.Collections.Generic;

namespace backend.Models
{
    public class CargoArea : BaseEntity
    {
        public long IdCargo { get; set; }
        public Cargo? Cargo { get; set; }

        public long IdArea { get; set; }
        public Area? Area { get; set; }
    }
}
