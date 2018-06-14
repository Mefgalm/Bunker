using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bunker.Business.Entities
{
    public class Team
    {
        public Team()
        {
            Players = new List<PlayerTeam>();
        }
        
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        public ICollection<PlayerTeam> Players { get; set; }
    }
}