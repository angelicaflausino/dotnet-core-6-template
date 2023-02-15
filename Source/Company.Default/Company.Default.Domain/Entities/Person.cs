using Company.Default.Domain.Contracts.Base;
using Company.Default.Domain.Enumerables;

namespace Company.Default.Domain.Entities
{
    public class Person : EntityBase<long>
    {
        public PersonTypeEnum PersonType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public int Age { get; set; }
    }
}
