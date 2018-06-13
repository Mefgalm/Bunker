using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bunker.Business.Interfaces.ValidationAttributes;

namespace Bunker.Business.Interfaces.Requests
{
    public class RegisterRequest
    {
        [PlayerEmail]
        public string Email { get; set; }

        [PlayerFirstName]
        public string FirstName { get; set; }

        [PlayerLastName]
        public string LastName { get; set; }

        [PlayerNickName]
        public string NickName { get; set; }

        [PlayerPassword]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}