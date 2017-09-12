using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Siedlisko.Models.DataValidators
{
    public sealed class DateAheadValidator : ValidationAttribute
    {
        #region Fields and properties
        private readonly string _testedPropertyName;
        #endregion

        #region .ctor
        public
            DateAheadValidator(string testedPropertyName)
        {
            _testedPropertyName = testedPropertyName;
        }
        #endregion

        #region Private Methods
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value == null)
            {
                return new ValidationResult("Value is null");
            }

            if (validationContext == null)
            {
                return new ValidationResult("validationContext is null");
            }
            var testedProperty = validationContext.ObjectType.GetProperty(_testedPropertyName);

            var valueToTestAgainst = (DateTime)testedProperty.GetValue(validationContext.ObjectInstance);

            var date = (DateTime)value;

            if (date > valueToTestAgainst)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Zła data końca pobytu.");
        } 
        #endregion
    }

}
