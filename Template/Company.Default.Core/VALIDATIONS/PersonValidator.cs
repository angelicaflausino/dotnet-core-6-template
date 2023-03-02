using $ext_safeprojectname$.Domain.Entities;
using FluentValidation;
using ValidationMessages = $ext_safeprojectname$.Domain.Resources.Validations;

namespace $safeprojectname$.Validations
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            CheckType();
            CheckFirstName();
            CheckLastName();
            CheckDateBirth();
            CheckAge();

            RuleSet("Update", () => CheckId());
        }

        private void CheckId()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage(ValidationMessages.FieldRequired)
                .GreaterThan(0).WithMessage(ValidationMessages.InvalidIdentifier);
        }

        private void CheckType()
        {
            RuleFor(x => x.PersonType).NotNull().WithMessage(ValidationMessages.FieldRequired);
        }

        private void CheckFirstName()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(ValidationMessages.FieldRequired)
                .MaximumLength(256).WithMessage(ValidationMessages.MaxLengthAllowed);
        }

        private void CheckLastName()
        {
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(ValidationMessages.FieldRequired)
                .MaximumLength(256).WithMessage(ValidationMessages.MaxLengthAllowed);
        }

        private void CheckDateBirth()
        {
            RuleFor(x => x.DateBirth).NotEqual(DateTime.MinValue).WithMessage(ValidationMessages.FieldRequired);
        }

        private void CheckAge()
        {
            RuleFor(x => x.Age).GreaterThan(0).WithMessage(ValidationMessages.FieldRequired);
        }
    }
}
