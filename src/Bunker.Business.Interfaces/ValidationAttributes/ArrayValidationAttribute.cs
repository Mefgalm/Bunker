using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Bunker.Business.Interfaces.ValidationAttributes
{
    public class ArrayValidationAttribute : ValidationAttribute
    {
        private readonly IEnumerable<ValidationAttribute> _validationAttributes;

        public ArrayValidationAttribute(IEnumerable<ValidationAttribute> validationAttributes)
        {
            _validationAttributes = validationAttributes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //var errorMessageBuilder = new StringBuilder();

            // foreach (var validationAttribute in _validationAttributes)
            // {
            //     var result = validationAttribute.GetValidationResult(value, validationContext);
            //
            //     if (result != null
            //      && !string.IsNullOrEmpty(result.ErrorMessage))
            //         errorMessageBuilder.AppendJoin(", ", result.ErrorMessage);
            // }

            string errorMessage = _validationAttributes.Select(x => x.GetValidationResult(value, validationContext))
                                 .Where(x => x != null && !string.IsNullOrEmpty(x.ErrorMessage))
                                 .Select(x => x.ErrorMessage)
                                 .Aggregate((x, y) => $"{x}, {y}");

            if(string.IsNullOrEmpty(errorMessage))                            
                return ValidationResult.Success;
            
            return new ValidationResult(errorMessage, new[] { validationContext.DisplayName });
        }
    }
}