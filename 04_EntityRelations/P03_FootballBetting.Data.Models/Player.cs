﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class Player
    {
        public Player()
        {
            Games = new List<Game>();
            PlayerStatistics = new List<PlayerStatistic>();
        }

        [Key]
        public int PlayerId { get; set; }

        public string Name { get; set; }

        public int SquadNumber { get; set; }

        public int TeamId { get; set; }

        public Team Team { get; set; }

        public int PositionId { get; set; }

        public Position Position{ get; set; }

        public bool IsInjured { get; set; }

        public ICollection<Game> Games { get; set; }

        public ICollection<PlayerStatistic> PlayerStatistics { get; set; }
    }
}