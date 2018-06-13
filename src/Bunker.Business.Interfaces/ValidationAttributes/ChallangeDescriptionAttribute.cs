using System.ComponentModel.DataAnnotations;

namespace Bunker.Business.Interfaces.ValidationAttributes
{
    public class ChallangeDescriptionAttribute : ArrayValidationAttribute
    {
        public ChallangeDescriptionAttribute()
            : base(new ValidationAttribute[] {new RequiredAttribute(), new StringLengthAttribute(1000)})
        {
        }
    }
}