﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Data.Data;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Repository;


namespace Tournament.Data.Repositories
{
    public class GameRepository:IGameRepository
    {
        private readonly TournamentContext _context;

        public GameRepository(TournamentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games.ToListAsync();
        }

        public async Task<Game> GetAsync(int id)
        {
            return await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Games.AnyAsync(g => g.Id == id);
        }

        public void Add(Game game)
        {
            _context.Games.Add(game);
        }

        public void Update(Game game)
        {
            _context.Games.Update(game);
        }

        public void Remove(Game game)
        {
            _context.Games.Remove(game);
        }


        public async Task<IEnumerable<Game>> GetByTitleAsync(string title)
        {
            return await _context.Games
                .Where(g => g.Title == title)
                .ToListAsync();


        }


    }
}

