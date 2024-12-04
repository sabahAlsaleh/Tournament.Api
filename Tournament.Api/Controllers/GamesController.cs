using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.DTO;
using Tournament.Core.Entities;
using Tournament.Core.Repository;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]

//    [Route("api/TournamentDetails/{TournamentDetailsID}/Games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GamesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper=mapper;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames()
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();
            var gamesDto = _mapper.Map< IEnumerable<GameDto>>(games);

            return Ok(gamesDto);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }
            var gameDto = _mapper.Map<GameDto>(game);

            return Ok(gameDto);
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameDto gameDto)
        {
            if (id != gameDto.Id)
            {
                return BadRequest();
            }
            var game = _mapper.Map<Game>(gameDto);
            _unitOfWork.GameRepository.Update(game);

            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.GameRepository.AnyAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(GameDto gameDto)
        {
            var game = _mapper.Map<Game>(gameDto);
            _unitOfWork.GameRepository.Add(game);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("GetGame", new { id = game.Id }, gameDto);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _unitOfWork.GameRepository.Remove(game);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
