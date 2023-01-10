namespace Company.Default.Domain.Base
{
    public interface IEntity
    {
        object Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Enabled { get; set; }
    }
}
