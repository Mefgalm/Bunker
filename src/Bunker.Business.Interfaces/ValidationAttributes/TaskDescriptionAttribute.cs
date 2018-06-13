using System.ComponentModel.DataAnnotations;

namespace Bunker.Business.Interfaces.ValidationAttributes
{
    public class TaskDescriptionAttribute : ArrayValidationAttribute
    {
        public TaskDescriptionAttribute()
            : base(new ValidationAttribute[] {new RequiredAttribute(), new StringLengthAttribute(2000)})
        {
        }
    }
}