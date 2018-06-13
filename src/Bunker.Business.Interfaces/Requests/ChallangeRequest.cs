using System;
using System.Text.RegularExpressions;
using Bunker.Business.Interfaces.ValidationAttributes;

namespace Bunker.Business.Interfaces.Requests
{
    public class ChallangeRequest
    {
        [ChallangeName]
        public string Name { get; set; }

        [ChallangeDescription]
        public string Description { get; set; }
    }
}