using System.ComponentModel;

namespace $safeprojectname$.Enumerables
{
    public enum PersonTypeEnum
    {
        [Description("Employee")]
        Employee = 1,
        [Description("Administrator")]
        Admin = 2,
        [Description("Guest")]
        Guest = 3
    }
}
