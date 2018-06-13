using System;
using System.Text.RegularExpressions;
using Bunker.Business.Interfaces.ValidationAttributes;

namespace Bunker.Business.Interfaces.Requests
{
    public class CompanyRequest
    {
        [CompanyName]
        public string Name { get; set; }
        
        [CompanyDescription]
        public string Desciprion { get; set; }                
    }
}