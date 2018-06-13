using System.ComponentModel.DataAnnotations;

namespace Bunker.Business.Interfaces.ValidationAttributes
{
    public class TaskAnswerAttribute : ArrayValidationAttribute
    {
        public TaskAnswerAttribute()
            : base(new ValidationAttribute[] {new StringLengthAttribute(200)})
        {
        }
    }
}