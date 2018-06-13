using System.ComponentModel.DataAnnotations;

namespace Bunker.Business.Interfaces.ValidationAttributes
{
    public class PlayerPasswordAttribute : ArrayValidationAttribute
    {
        public PlayerPasswordAttribute()
            : base(new ValidationAttribute[] {new RegularExpressionAttribute("^[A-Za-z0-9]{6,16}$")})
        {
        }
    }
}