using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bunker.Business.Entities
{
    public class CompanyJoinInfo
    {
        [Key]
        public int CompanyId { get; set; }
        
        [Required]
        public string Key { get; set; }
        
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
    }
}