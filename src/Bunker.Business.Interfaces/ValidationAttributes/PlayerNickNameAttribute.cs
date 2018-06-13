using System.ComponentModel.DataAnnotations;

namespace Bunker.Business.Interfaces.ValidationAttributes
{
    public class PlayerNickNameAttribute : ArrayValidationAttribute
    {
        public PlayerNickNameAttribute()
            : base(new ValidationAttribute[] {new RegularExpressionAttribute("^[A-Za-z ]{1,16}$")})
        {
        }
    }
}