﻿using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bunker.Business.Entities
{
    public class PlayerTeam
    {
        public int PlayerId { get; set; }
        
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }
        
        public int TeamId { get; set; }
        
        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }
    }
}