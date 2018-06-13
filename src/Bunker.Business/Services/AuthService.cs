using System.ComponentModel.DataAnnotations;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;
using Bunker.Business.Interfaces.Services;

namespace Bunker.Business.Services
{
    public class AuthService : BaseService, IAuthService
    {
        public AuthService(BunkerDbContext dbContext) : base(dbContext)
        {
        }
        
        public BaseResponse<LoginResponse> Login(string email, string password)
        {
            
            
            throw new System.NotImplementedException();
        }

        public BaseResponse<object> Register(RegisterRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}