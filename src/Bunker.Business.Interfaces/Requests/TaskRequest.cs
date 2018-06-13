using System.Collections.Generic;
using Bunker.Business.Interfaces.ValidationAttributes;

namespace Bunker.Business.Interfaces.Requests
{
    public class TaskRequest
    {
        [TaskName]
        public string Name { get; set; }

        [TaskDescription]
        public string Description { get; set; }

        [TaskScore]
        public int Score { get; set; }

        public bool NoAnswer { get; set; }

        [TaskAnswer]
        public string Answer { get; set; }
    }
}