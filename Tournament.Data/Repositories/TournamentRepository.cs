﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Data.Data;
using Microsoft.EntityFrameworkCore;


namespace Tournament.Data.Repositories
{
    public class TournamentRepository
    {
        private readonly TournamentContext _context;

        public TournamentRepository(TournamentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TournamentDetails>> GetAllAsync()
        {
            return await _context.TournamentDetails.Include(t => t.Games).ToListAsync();
        }

        public async Task<TournamentDetails> GetAsync(int id)
        {
            return await _context.TournamentDetails.Include(t => t.Games).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.TournamentDetails.AnyAsync(t => t.Id == id);
        }

        public void Add(TournamentDetails tournament)
        {
            _context.TournamentDetails.Add(tournament);
        }

        public void Update(TournamentDetails tournament)
        {
            _context.TournamentDetails.Update(tournament);
        }

        public void Remove(TournamentDetails tournament)
        {
            _context.TournamentDetails.Remove(tournament);
        }
    }
}
