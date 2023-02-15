using System.ComponentModel.DataAnnotations;

namespace Company.Default.Domain.Contracts.Base
{
    public abstract class EntityBase<T> : IEntity
    {
        object IEntity.Id { get => Id; set { } }

        [Required]
        public virtual T Id { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
        public virtual bool Enabled { get; set; }
    }
}
