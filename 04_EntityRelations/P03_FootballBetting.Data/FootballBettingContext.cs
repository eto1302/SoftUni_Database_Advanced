using System;
using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Models;

namespace P03_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<User> Users { get; set; }

        public FootballBettingContext()
        {
        }

        public FootballBettingContext(DbContextOptions<FootballBettingContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=.\SQLEXPRESS;Database=FootballBettingSystem;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>().HasKey(t => t.TeamId);
            modelBuilder.Entity<Color>().HasKey(t => t.ColorId);
            modelBuilder.Entity<Town>().HasKey(t => t.TownId);
            modelBuilder.Entity<Country>().HasKey(t => t.CountryId);
            modelBuilder.Entity<Player>().HasKey(t => t.PlayerId);
            modelBuilder.Entity<Position>().HasKey(t => t.PositionId);
            modelBuilder.Entity<Game>().HasKey(t => t.GameId);
            modelBuilder.Entity<Bet>().HasKey(t => t.BetId);
            modelBuilder.Entity<User>().HasKey(t => t.UserId);
        }
    }
}