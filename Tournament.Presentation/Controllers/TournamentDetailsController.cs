using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System.ComponentModel.DataAnnotations;
using Tournament.Core.DTO;
using Tournament.Core.DTOs;

namespace Tournament.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public TournamentDetailsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;

        }

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails([FromQuery] bool includeMatches = false, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string sortOrder = null, [FromQuery] string filter = null)
        {
            var tournaments = await _serviceManager.TournamentService.GetTournamentsAsync(includeMatches, page, pageSize, sortOrder, filter);
            return Ok(tournaments);
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
        {
            var tournament = await _serviceManager.TournamentService.GetTournamentByIdAsync(id);
            return Ok(tournament);
        }

        // POST: api/TournamentDetails
        [HttpPost]
        public async Task<ActionResult<TournamentCreateDto>> PostTournamentDetails(TournamentCreateDto tournamentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _serviceManager.TournamentService.AddTournamentAsync(tournamentDto);

            return CreatedAtAction(nameof(GetTournamentDetails), new { id = tournamentDto.Id }, tournamentDto);
        }

        // PUT: api/TournamentDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentDto tournamentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _serviceManager.TournamentService.UpdateTournamentAsync(id, tournamentDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the resource.");
            }
        }

        // PATCH: api/TournamentDetails/{id}
        [HttpPatch("{tournamentId}")]
        public async Task<ActionResult<TournamentDto>> PatchTournament(int tournamentId, [FromBody] JsonPatchDocument<TournamentDto> patchDocument)
        {
            try
            {
                var updatedTournament = await _serviceManager.TournamentService.PatchTournamentAsync(tournamentId, patchDocument);
                return Ok(updatedTournament);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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


        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            try
            {
                await _serviceManager.TournamentService.DeleteTournamentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the resource.");
            }
        }
    }
}
