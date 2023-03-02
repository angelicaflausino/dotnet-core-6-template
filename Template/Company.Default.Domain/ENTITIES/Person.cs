using $safeprojectname$.Contracts.Base;
using $safeprojectname$.Enumerables;

namespace $safeprojectname$.Entities
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
