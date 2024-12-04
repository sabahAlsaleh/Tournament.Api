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
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TournamentDetailsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails([FromQuery] bool includeMatches=false)
        {
            IEnumerable<TournamentDetails> tournaments;

            if (includeMatches)
            {
                tournaments = await _unitOfWork.TournamentRepository.GetAllIncludingMatchesAsync();
            }
            else
            {
                tournaments = await _unitOfWork.TournamentRepository.GetAllAsync();
            }
            var tournamentsDtos= _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return Ok(tournamentsDtos);
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournamentDetails(int id)
        {
            var tournamentDetails = await _unitOfWork.TournamentRepository.GetAsync(id);

            if (tournamentDetails == null)
            {
                return NotFound();
            }
            var tournamentDto = _mapper.Map<TournamentDto>(tournamentDetails);


            return Ok(tournamentDto);
        }

        // PUT: api/TournamentDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentDto tournamentDto)
        {
            if (id != tournamentDto.Id)
            {
                return BadRequest("The provided ID does not match the resource ID.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var tournamentDetails = _mapper.Map<TournamentDetails>(tournamentDto);
            _unitOfWork.TournamentRepository.Update(tournamentDetails);
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _unitOfWork.TournamentRepository.AnyAsync(id))
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

        // POST: api/TournamentDetails
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournamentDetails(TournamentDto tournamentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var tournamentDetails = _mapper.Map<TournamentDetails>(tournamentDto);
            _unitOfWork.TournamentRepository.Add(tournamentDetails);
            try
            {
                await _unitOfWork.CompleteAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while saving the resource.");
            }
            return CreatedAtAction("GetTournamentDetails", new { id = tournamentDetails.Id }, tournamentDto);
        }

        // PATCH method for updating partial fields in TournamentDetails
        [HttpPatch("{tournamentId}")]
        public async Task<ActionResult<TournamentDto>> PatchTournament( int tournamentId, [FromBody]JsonPatchDocument<TournamentDto> patchDocument)
        {
            if(patchDocument == null ) return BadRequest("Invalid patch document");

            // Retrieve the tournament details from the database
            var tournamentDetails = await _unitOfWork.TournamentRepository.GetAsync(tournamentId);
            if (tournamentDetails == null) return NotFound();

            // Map the retrieved tournament entity to a DTO
            var tournamentDto = _mapper.Map<TournamentDto>(tournamentDetails);

            // Apply the patch to the DTO, validating the model state
            patchDocument.ApplyTo(tournamentDto, ModelState);

            if(!ModelState.IsValid) {return BadRequest(ModelState);}

            // Map the patched DTO back to the entity and update the repository
            _mapper.Map(tournamentDto, tournamentDetails);
            _unitOfWork.TournamentRepository.Update(tournamentDetails);

            try
            {
                await _unitOfWork.CompleteAsync();

            }
            catch (Exception)
            {

                return StatusCode(500, "An error occurred while updating the resource.");
            }

            return Ok( tournamentDto );


        }


        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            var tournamentDetails = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (tournamentDetails == null)
            {
                return NotFound();
            }

            _unitOfWork.TournamentRepository.Remove(tournamentDetails);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
    }
}
