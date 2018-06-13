using System.ComponentModel.DataAnnotations;

namespace Bunker.Business.Interfaces.ValidationAttributes
{
    public class PlayerFirstNameAttribute : ArrayValidationAttribute
    {
        public PlayerFirstNameAttribute()
            : base(new ValidationAttribute[] {new RegularExpressionAttribute("^[A-Za-z ]{1,16}$")})
        {
        }
    }
}