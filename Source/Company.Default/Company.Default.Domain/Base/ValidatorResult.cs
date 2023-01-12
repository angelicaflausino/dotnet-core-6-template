namespace Company.Default.Domain.Base
{
    public class ValidatorResult
    {
        public ValidatorResult(bool isValid, IList<ValidatorError> errors)
        {
            IsValid = isValid;
            Errors = errors;
        }

        public bool IsValid { get; set; }
        public IList<ValidatorError> Errors { get; set;}
    }

    public class ValidatorError
    {
        public ValidatorError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }
}
