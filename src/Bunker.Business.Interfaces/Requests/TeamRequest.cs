using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Bunker.Business.Interfaces.Models;
using Bunker.Business.Interfaces.ValidationAttributes;

namespace Bunker.Business.Interfaces.Requests
{
    public class TeamRequest
    {
        [TeamName]
        public string Name { get; set; }                
    }
}