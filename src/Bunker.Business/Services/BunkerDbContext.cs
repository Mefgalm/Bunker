﻿using Bunker.Business.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bunker.Business.Services
{
    public class BunkerDbContext : DbContext
    {
        public DbSet<Challange>     Challanges     { get; set; }
        public DbSet<ChallangeTeam> ChallangeTeams { get; set; }
        public DbSet<Company>       Companies      { get; set; }
        public DbSet<CompanyPlayer> CompanyPlayers { get; set; }
        public DbSet<Player>        Players        { get; set; }
        public DbSet<PlayerTeam>    PlayerTeams    { get; set; }
        public DbSet<Task>          Tasks          { get; set; }
        public DbSet<Team>          Teams          { get; set; }

        public BunkerDbContext(DbContextOptions<BunkerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChallangeTeam>()
                        .HasKey(x => new {x.ChallangeId, x.TeamId});

            modelBuilder.Entity<CompanyPlayer>()
                        .HasKey(x => new {x.CompanyId, x.PlayerId});

            modelBuilder.Entity<PlayerTeam>()
                        .HasKey(x => new {x.PlayerId, x.TeamId});

            modelBuilder.Entity<PlayerTask>()
                        .HasKey(x => new {x.PlayerId, x.TaskId});

            base.OnModelCreating(modelBuilder);
        }
    }
}