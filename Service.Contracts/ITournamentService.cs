using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.DTO;
using Tournament.Core.DTOs;

namespace Service.Contracts
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentDtoWithGame>> GetTournamentsAsync(bool includeMatches, int page, int pageSize, string sortOrder, string filter);
        Task<TournamentDto> GetTournamentByIdAsync(int id);
        Task AddTournamentAsync(TournamentDto tournamentDto);
        Task UpdateTournamentAsync(int id, TournamentDto tournamentDto);
        Task DeleteTournamentAsync(int id);
        Task<TournamentDto> PatchTournamentAsync(int tournamentId, JsonPatchDocument<TournamentDto> patchDocument);
    }
}
