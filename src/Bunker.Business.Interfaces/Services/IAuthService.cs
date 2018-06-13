using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.Requests;

namespace Bunker.Business.Interfaces.Services
{
    public interface IAuthService
    {
        BaseResponse<LoginResponse> Login(string email, string password);
        BaseResponse<object>        Register(RegisterRequest request);

        //forgot password
    }
}