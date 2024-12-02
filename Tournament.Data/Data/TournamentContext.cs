﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public class TournamentContext : DbContext
    {
        public TournamentContext (DbContextOptions<TournamentContext> options)
            : base(options)
        {
        }

        public DbSet<Tournament.Core.Entities.TournamentDetails> TournamentDetails { get; set; } = default!;
    }
}
