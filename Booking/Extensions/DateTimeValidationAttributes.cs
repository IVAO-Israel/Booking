using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Booking.Extensions.Validation
{
    public class DateTimeBeforeAttribute : ValidationAttribute
    {
        private readonly string _comparisonPropertyName;
        public DateTimeBeforeAttribute(string comparisonPropertyName)
        {
            _comparisonPropertyName = comparisonPropertyName;
            ErrorMessage = "{0} must be before " + _comparisonPropertyName;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Get the comparison property
            PropertyInfo? comparisonProperty = validationContext.ObjectType.GetProperty(_comparisonPropertyName);
            if (comparisonProperty == null)
            {
                return new ValidationResult($"Unknown property: {_comparisonPropertyName}");
            }
            var comparisonValue = comparisonProperty.GetValue(validationContext.ObjectInstance);
            if (value is DateTime currentDate && comparisonValue is DateTime comparisonDate)
            {
                if (currentDate >= comparisonDate)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }
    }
    public class DateTimeAfterAttribute : ValidationAttribute
    {
        private readonly string _startDatePropertyName;
        public DateTimeAfterAttribute(string startDatePropertyName)
        {
            _startDatePropertyName = startDatePropertyName;
            ErrorMessage = "{0} must be after " + _startDatePropertyName;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Get the start date property
            PropertyInfo? startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            if (startDateProperty == null)
            {
                return new ValidationResult($"Unknown property: {_startDatePropertyName}");
            }
            var startDateValue = startDateProperty.GetValue(validationContext.ObjectInstance);
            if (value is DateTime endDate && startDateValue is DateTime startDate)
            {
                if (endDate <= startDate)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }
    }
}
