using System.ComponentModel.DataAnnotations.Schema;

namespace Bunker.Business.Entities
{
    public class ChallangeTeam
    {
        public int ChallangeId { get; set; }
        
        [ForeignKey(nameof(ChallangeId))]
        public Challange Challange { get; set; }
        
        public int TeamId { get; set; }
        
        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }
    }
}