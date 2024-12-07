using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.DTO;

namespace Service.Contracts
{
    public interface IGameService
    {
        Task<IEnumerable<GameDto>> GetAllGamesAsync();
        Task<GameDto> GetGameByIdAsync(int id);
        Task<GameDto> AddGameAsync(GameDto gameDto);
        Task UpdateGameAsync(int id, GameDto gameDto);
        Task DeleteGameAsync(int id);
        Task<GameDto> PatchGameAsync(int gameId, JsonPatchDocument<GameDto> patchDocument);
        Task<IEnumerable<GameDto>> GetGamesByTitleAsync(string title);
    }
}
