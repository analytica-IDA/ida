using System;

namespace backend.Models
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public DateTime DtUltimaAtualizacao { get; set; } = DateTime.UtcNow;
    }
}
