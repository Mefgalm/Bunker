using Bunker.Business.Interfaces.Requests;
using Bunker.Business.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bunker.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpGet]
        public IActionResult Login() => View();
        
        
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var loginResponse = _authService.Login(email, password);

            if (!loginResponse.Ok)
            {
                ModelState.AddModelError(string.Empty, loginResponse.Info);
                return View();
            }
            
            return View();
        }

        [HttpGet]
        public IActionResult Register(RegisterRequest request)
        {
            
            
            return View();
        }
    }
}