using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SBDProjekt.Validation
{
    public class BeforeEndDateAttribute : ValidationAttribute
    {
        public string EndDatePropertyName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo endDateProperty = validationContext.ObjectType.GetProperty(EndDatePropertyName);

            DateTime endDate = (DateTime)endDateProperty.GetValue(validationContext.ObjectInstance, null);

            DateTime startDate = (DateTime)value;

            return startDate < endDate ? ValidationResult.Success : new ValidationResult("Start date cannot be after end date");
        }
    }
}
