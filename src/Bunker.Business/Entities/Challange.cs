using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bunker.Business.Entities
{
    public class Challange
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [StringLength(10000)]
        public string Desciprion { get; set; }
        
        public ICollection<Task> Tasks { get; set; }
        
        
    }
}