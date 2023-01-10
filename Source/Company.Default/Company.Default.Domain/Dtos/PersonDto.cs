using Company.Default.Domain.Enumerables;

namespace Company.Default.Domain.Dtos
{
    public class PersonDto
    {
        public PersonDto()
        {

        }
        public long Id { get; set; }
        public PersonTypeEnum PersonType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime DateBirth { get; set; }
        public int Age { get => this.Age == 0 ? GetAge() : this.Age; set => this.Age = value; }

        public int GetAge()
        {
            DateTime now = DateTime.Today;
            DateTime birth = DateBirth;
            int age = now.Year - birth.Year;

            if (now < birth.AddYears(age)) age--;

            return age;
        }
    }
}
