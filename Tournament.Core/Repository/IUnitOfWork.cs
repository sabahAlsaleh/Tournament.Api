using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Core.Repository
{
    public interface IUnitOfWork
    {
        ITournamentRepository TournamentRepository { get; }
        IGameRepository GameRepository { get; }
        Task CompleteAsync();

    }
}
