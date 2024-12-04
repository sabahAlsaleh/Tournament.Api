using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
                return BadRequest("The provided ID does not match the resource ID.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                    return StatusCode(500, "An error occurred while updating the resource.");
                }
            }

            return NoContent();
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(GameDto gameDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var game = _mapper.Map<Game>(gameDto);
            _unitOfWork.GameRepository.Add(game);
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while saving the resource.");
            }

            return CreatedAtAction("GetGame", new { id = game.Id }, gameDto);
        }

        [HttpPatch("{gameId}")]
        public async Task<ActionResult<GameDto>> PatchGame( int gameId, [ FromBody]JsonPatchDocument<GameDto> patchDocument)
        {
            if(patchDocument == null) return BadRequest("Invalid patch document ");

            // Retrieve the game from database
            var game= await _unitOfWork.GameRepository.GetAsync(gameId);
            if (game == null) return NotFound();

            // Map the retrieved game entity to a DTO
            var gameDto = _mapper.Map<GameDto>(game);

            // Apply the patch to the DTO , Validating the model state
            patchDocument.ApplyTo(gameDto, ModelState);
            if(!ModelState.IsValid) { return BadRequest(ModelState); }

            //Map the patched DTO back to the entity and update the repository
            _mapper.Map(gameDto, game);
            _unitOfWork.GameRepository.Update(game);


            try
            {
                // Attempt to save changes to the database
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the resource.");
            }

            return Ok(gameDto);
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

        [HttpGet("title/{title}")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGameByTitle( string title)
        {
            var games = await _unitOfWork.GameRepository.GetByTitleAsync(title);
            if (games == null || !games.Any()) return NotFound("No games found with the specified title.");


            var gamesDtos = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gamesDtos);
        }



    }
}
