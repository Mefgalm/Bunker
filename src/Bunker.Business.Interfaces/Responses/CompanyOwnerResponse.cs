using System.Dynamic;

namespace Bunker.Business.Interfaces.Models
{
    public class CompanyOwnerResponse
    {
        public int    Id        { get; set; }
        public string NickName  { get; set; }
        public string FirstName { get; set; }
        public string LastName  { get; set; }        
    }
}