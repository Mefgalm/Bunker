using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bunker.Business.Entities
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(2000)]
        public string Description { get; set; }
        
        public int Score { get; set; }
        
        [StringLength(200)]
        public string Answer { get; set; }
        
        public int? PlayerId { get; set; }
             
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }
    }
}