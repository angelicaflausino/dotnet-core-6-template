using $safeprojectname$.Enumerables;

namespace $safeprojectname$.Dtos
{
    public class PersonDto
    {
        public long Id { get; set; }
        public PersonTypeEnum PersonType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime DateBirth { get; set; }

        private int _age = 0;
        public int Age
        {
            get
            {
                if (_age == 0) 
                    SetAge();

                return _age;
            }            
        }

        private void SetAge()
        {
            if (DateBirth == DateTime.MinValue)
                _age = 0;

            DateTime now = DateTime.Today;
            DateTime birth = DateBirth;
            int age = now.Year - birth.Year;

            if (now.DayOfYear < birth.DayOfYear) age--;

            _age = age;
        }
    }
}
