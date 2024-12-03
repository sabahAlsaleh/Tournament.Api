using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repository;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly TournamentContext _context;
        public ITournamentRepository TournamentRepository { get; private set; }
        public IGameRepository GameRepository { get; private set; }

        public UnitOfWork(TournamentContext context)
        {
            _context = context;
            TournamentRepository = new TournamentRepository(_context);
            GameRepository= new GameRepository(_context);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }






    }
}
