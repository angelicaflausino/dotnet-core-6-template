using Company.Default.Domain.Base;
using Company.Default.Domain.Enumerables;

namespace Company.Default.Domain.Filters
{
    public class PersonFilterParameter : FilterParameterBase
    {
        public PersonFilterParameter()
        {
            //Override default values from base class
            this.Page = 100;
            this.SortBy = "Name asc";
            this.Size = 50;
        }

        public PersonTypeEnum? Type { get; set; }
        public string Name { get; set; }
        public DateTime? StartBirthDate { get; set; }
        public DateTime? EndBirthDate { get; set; }
        public DateTime? StartCreatedDate { get; set; }
        public DateTime? EndCreatedDate { get; set; }
        public int? StartAge { get; set; }
        public int? EndAge { get; set; }
    }
}
