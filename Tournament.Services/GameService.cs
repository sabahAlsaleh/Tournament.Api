using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Service.Contracts;
using Tournament.Core.DTO;
using Tournament.Core.Entities;
using Tournament.Core.Repository;

namespace Tournament.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GameDto>> GetAllGamesAsync()
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GameDto>>(games);
        }

        public async Task<GameDto> GetGameByIdAsync(int id)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);
            if (game == null)
                throw new KeyNotFoundException($"Game with ID {id} not found.");

            return _mapper.Map<GameDto>(game);
        }

        public async Task<GameDto> AddGameAsync(GameDto gameDto)
        {
            var game = _mapper.Map<Game>(gameDto);
            _unitOfWork.GameRepository.Add(game);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<GameDto>(game);
        }

        public async Task UpdateGameAsync(int id, GameDto gameDto)
        {
            if (id != gameDto.Id)
                throw new ArgumentException("The provided ID does not match the resource ID.");

            var game = await _unitOfWork.GameRepository.GetAsync(id);
            if (game == null)
                throw new KeyNotFoundException($"Game with ID {id} not found.");

            _mapper.Map(gameDto, game);
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteGameAsync(int id)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);
            if (game == null)
                throw new KeyNotFoundException($"Game with ID {id} not found.");

            _unitOfWork.GameRepository.Remove(game);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<GameDto> PatchGameAsync(int gameId, JsonPatchDocument<GameDto> patchDocument)
        {
            if (patchDocument == null)
                throw new ArgumentException("Invalid patch document.");

            var game = await _unitOfWork.GameRepository.GetAsync(gameId);
            if (game == null)
                throw new KeyNotFoundException($"Game with ID {gameId} not found.");

            var gameDto = _mapper.Map<GameDto>(game);
            patchDocument.ApplyTo(gameDto);

            _mapper.Map(gameDto, game);
            _unitOfWork.GameRepository.Update(game);
            await _unitOfWork.CompleteAsync();

            return gameDto;
        }

        public async Task<IEnumerable<GameDto>> GetGamesByTitleAsync(string title)
        {
            var games = await _unitOfWork.GameRepository.GetByTitleAsync(title);
            return _mapper.Map<IEnumerable<GameDto>>(games);
        }
    }
}
