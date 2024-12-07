using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Tournament.Core.DTO;

namespace Tournament.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public GamesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames()
        {
            var games = await _serviceManager.GameService.GetAllGamesAsync();
            return Ok(games);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            try
            {
                var game = await _serviceManager.GameService.GetGameByIdAsync(id);
                return Ok(game);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
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

            try
            {
                await _serviceManager.GameService.UpdateGameAsync(id, gameDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the resource.");
            }

            return NoContent();
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<GameDto>> PostGame(GameDto gameDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdGame = await _serviceManager.GameService.AddGameAsync(gameDto);
                return CreatedAtAction("GetGame", new { id = createdGame.Id }, createdGame);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while saving the resource.");
            }
        }

        // PATCH: api/Games/{gameId}
        [HttpPatch("{gameId}")]
        public async Task<ActionResult<GameDto>> PatchGame(int gameId, [FromBody] JsonPatchDocument<GameDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest("Invalid patch document");
            }

            try
            {
                var updatedGame = await _serviceManager.GameService.PatchGameAsync(gameId, patchDocument);
                return Ok(updatedGame);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the resource.");
            }
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            try
            {
                await _serviceManager.GameService.DeleteGameAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the resource.");
            }
        }

        // GET: api/Games/title/{title}
        [HttpGet("title/{title}")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGameByTitle(string title)
        {
            var games = await _serviceManager.GameService.GetGamesByTitleAsync(title);
            if (games == null || !games.Any())
            {
                return NotFound("No games found with the specified title.");
            }

            return Ok(games);
        }
    }
}
