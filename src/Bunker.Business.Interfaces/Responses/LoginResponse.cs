using System.Collections.Generic;

namespace Bunker.Business.Interfaces.Responses
{
    public class LoginResponse
    {
        public string              PlayerId { get; set; }
        public IEnumerable<string> Roles  { get; set; }
    }
}