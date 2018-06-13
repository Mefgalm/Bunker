using System.ComponentModel.DataAnnotations;

namespace Bunker.Business.Interfaces.ValidationAttributes
{
    public class CompanyNameAttribute : ArrayValidationAttribute
    {
        public CompanyNameAttribute()
            : base(new ValidationAttribute[] {new RequiredAttribute(), new RegularExpressionAttribute(@"^[A-Za-z0-9 ]{3,100}$")})
        {
        }
    }
}