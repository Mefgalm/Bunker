using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Bunker.Business.Interfaces.Models;

namespace Bunker.Business.Services
{
    public class BaseService
    {
        protected BunkerDbContext _dbContext;

        public BaseService(BunkerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected BaseResponse<T> Validate<T>(object obj)
        {
            var resultList = new List<ValidationResult>();

            if (!Validator.TryValidateObject(obj, new ValidationContext(obj), resultList))
                return BaseResponse<T>.Fail(string.Join(", ", resultList.Select(x => x.ErrorMessage)));
            
            return BaseResponse<T>.Success();
        }
    }
}