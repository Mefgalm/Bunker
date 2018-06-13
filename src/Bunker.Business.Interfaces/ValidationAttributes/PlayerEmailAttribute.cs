using System.ComponentModel.DataAnnotations;

namespace Bunker.Business.Interfaces.ValidationAttributes
{
    public class PlayerEmailAttribute : ArrayValidationAttribute
    {
        public PlayerEmailAttribute()
            : base(new ValidationAttribute[] {new RequiredAttribute(), new EmailAddressAttribute(), new StringLengthAttribute(60)})
        {
        }
    }
}