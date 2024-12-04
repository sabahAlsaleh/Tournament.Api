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
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournamentDetails()
        {
            var tournaments = await _unitOfWork.TournamentRepository.GetAllAsync();
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
                return BadRequest();
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
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TournamentDetails
        [HttpPost]
        public async Task<ActionResult<TournamentDto>> PostTournamentDetails(TournamentDto tournamentDto)
        {
            var tournamentDetails = _mapper.Map<TournamentDetails>(tournamentDto);
            _unitOfWork.TournamentRepository.Add(tournamentDetails);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction("GetTournamentDetails", new { id = tournamentDetails.Id }, tournamentDto);
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
