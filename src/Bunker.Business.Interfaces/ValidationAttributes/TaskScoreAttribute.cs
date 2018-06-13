using System.ComponentModel.DataAnnotations;

namespace Bunker.Business.Interfaces.ValidationAttributes
{
    public class TaskScoreAttribute : ArrayValidationAttribute
    {
        public TaskScoreAttribute()
            : base(new ValidationAttribute[] {new RangeAttribute(1, 1000)})
        {
        }
    }
}