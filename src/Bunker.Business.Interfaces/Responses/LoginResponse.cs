using System.Collections.Generic;

namespace Bunker.Business.Interfaces.Models
{
    public class LoginResponse
    {
        public string              UserId { get; set; }
        public IEnumerable<string> Roles  { get; set; }
    }
}