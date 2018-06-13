using System.ComponentModel.DataAnnotations;

namespace Bunker.Business.Interfaces.ValidationAttributes
{
    public class CompanyDescriptionAttribute : ArrayValidationAttribute
    {
        public CompanyDescriptionAttribute()
            : base(new ValidationAttribute[] { new StringLengthAttribute(1000)})
        {
        }
    }
}