using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bunker.Business.Entities
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(1000)]
        public string Desciprion { get; set; }
        
        public ICollection<Challange> Challanges { get; set; }                
   
        public ICollection<CompanyPlayer> Players { get; set; }
    }
}