using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SBDProjekt.Validation
{
    public class AfterStartDateAttribute : ValidationAttribute
    {
        public string StartDatePropertyName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo startDateProperty = validationContext.ObjectType.GetProperty(StartDatePropertyName);
            
            DateTime startDate = (DateTime)startDateProperty.GetValue(validationContext.ObjectInstance, null);

            DateTime endDate = (DateTime)value;

            return startDate < endDate ? ValidationResult.Success : new ValidationResult("End date cannot be before start date");
        }
    }
}
