using Bunker.Business.Interfaces.ValidationAttributes;

namespace Bunker.Business.Interfaces.Requests
{
    public class PlayerRequest
    {
        [PlayerFirstName]
        public string FirstName { get; set; }

        [PlayerLastName]
        public string LastName { get; set; }

        [PlayerNickName]
        public string NickName { get; set; }
    }
}