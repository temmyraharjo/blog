using System.ComponentModel.DataAnnotations;

namespace Blog.Core.Models
{
    public abstract class Entity
    {
        [Key]
        public long Id { get; set; }
        [ConcurrencyCheck]
        public byte[] RowVersion { get; set; }
    }
}
