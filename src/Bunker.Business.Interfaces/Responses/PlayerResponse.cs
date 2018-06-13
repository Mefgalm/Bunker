using System.Net.NetworkInformation;

namespace Bunker.Business.Interfaces.Models
{
    public class PlayerResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }        
    }
}