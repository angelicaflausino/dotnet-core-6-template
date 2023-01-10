using System.ComponentModel.DataAnnotations;

namespace Company.Default.Domain.Base
{
    public abstract class EntityBase<T> : IEntity
    {
        object IEntity.Id { get => Id; set { } }

        [Required]
        public T Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Enabled { get; set; }
    }
}
