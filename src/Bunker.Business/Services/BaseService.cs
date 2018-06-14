using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Bunker.Business.Interfaces.Infrastructure;
using Bunker.Business.Interfaces.Models;

namespace Bunker.Business.Services
{
    public class BaseService
    {
        protected readonly BunkerDbContext _dbContext;
        protected readonly IErrorMessageProvider _errorMessageProvider;

        public BaseService(BunkerDbContext dbContext, IErrorMessageProvider errorMessageProvider)
        {
            _dbContext = dbContext;
            _errorMessageProvider = errorMessageProvider;
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