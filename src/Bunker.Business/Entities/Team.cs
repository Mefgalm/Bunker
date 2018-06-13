using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bunker.Business.Entities
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        public ICollection<PlayerTeam> Players { get; set; }
    }
}