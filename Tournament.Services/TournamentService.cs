using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.DTO;
using Tournament.Core.DTOs;
using Tournament.Core.Entities;
using Tournament.Core.Repository;

namespace Tournament.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TournamentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        
        /*
        public async Task<IEnumerable<TournamentDto>> GetTournamentsAsync(bool includeMatches, int page, int pageSize)
        {
            var tournaments = includeMatches
                ? await _unitOfWork.TournamentRepository.GetAllIncludingMatchesAsync()
                : await _unitOfWork.TournamentRepository.GetAllAsync();

            var totalItems = tournaments.Count();


            pageSize = pageSize > 100 ? 100 : (pageSize <= 0 ? 20 : pageSize);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var paginatedTournaments = tournaments.Skip((page - 1) * pageSize).Take(pageSize);

            // Prepare metadata
            var metadata = new
            {
                totalPages,
                pageSize,
                currentPage = page,
                totalItems
            };

            var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(paginatedTournaments);
            return tournamentDtos;
        }
        */

        public async Task<IEnumerable<TournamentDtoWithGame>> GetTournamentsAsync(bool includeMatches, int page, int pageSize, string sortOrder, string filter = null)
        {
            var tournaments = includeMatches
                ? await _unitOfWork.TournamentRepository.GetAllIncludingMatchesAsync()
                : await _unitOfWork.TournamentRepository.GetAllAsync();

            // Apply filtering
            if (!string.IsNullOrWhiteSpace(filter))
            {
                tournaments = tournaments.Where(t => t.Title.Contains(filter, StringComparison.OrdinalIgnoreCase));
            }

            // Apply sorting
            tournaments = sortOrder switch
            {
                "title_desc" => tournaments.OrderByDescending(t => t.Title),
                "start_date" => tournaments.OrderBy(t => t.StartDate),
                "start_date_desc" => tournaments.OrderByDescending(t => t.StartDate),
                _ => tournaments.OrderBy(t => t.Title) // Default sorting by title
            };

            // Pagination logic
            var totalItems = tournaments.Count();
            pageSize = pageSize > 100 ? 100 : (pageSize <= 0 ? 20 : pageSize);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var paginatedTournaments = tournaments.Skip((page - 1) * pageSize).Take(pageSize);

            // Prepare metadata (optional)
            var metadata = new
            {
                totalPages,
                pageSize,
                currentPage = page,
                totalItems
            };

            // Map to DTO
            return _mapper.Map<IEnumerable<TournamentDtoWithGame>>(paginatedTournaments);
        }

        

        public async Task<TournamentDto> GetTournamentByIdAsync(int id)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (tournament == null)
            {
                throw new KeyNotFoundException($"Tournament with ID {id} not found.");
            }
            return _mapper.Map<TournamentDto>(tournament);
        }

        public async Task AddTournamentAsync(TournamentDto tournamentDto)
        {
            var tournament = _mapper.Map<TournamentDetails>(tournamentDto);

            if (tournament == null)
            {
                throw new ArgumentException("Invalid tournament data.");
            }

            _unitOfWork.TournamentRepository.Add(tournament);
            await _unitOfWork.CompleteAsync();
        }


        public async Task DeleteTournamentAsync(int id)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (tournament == null)
            {
                throw new KeyNotFoundException($"Tournament with ID {id} not found.");
            }

            _unitOfWork.TournamentRepository.Remove(tournament);
            await _unitOfWork.CompleteAsync();
        }





        public async Task UpdateTournamentAsync(int id, TournamentDto tournamentDto)
        {
            if (id != tournamentDto.Id)
            {
                throw new ArgumentException("The provided ID does not match the tournament ID.");
            }

            var tournament = await _unitOfWork.TournamentRepository.GetAsync(id);
            if (tournament == null)
            {
                throw new KeyNotFoundException($"Tournament with ID {id} not found.");
            }

            // Map updated values from DTO to the entity
            _mapper.Map(tournamentDto, tournament);

            _unitOfWork.TournamentRepository.Update(tournament);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<TournamentDto> PatchTournamentAsync(int tournamentId, JsonPatchDocument<TournamentDto> patchDocument)
        {
            if (patchDocument == null)
                throw new ArgumentException("Invalid patch document");

            // Retrieve the tournament details from the database
            var tournamentDetails = await _unitOfWork.TournamentRepository.GetAsync(tournamentId);
            if (tournamentDetails == null)
                throw new KeyNotFoundException($"Tournament with ID {tournamentId} not found");

            // Map the retrieved tournament entity to a DTO
            var tournamentDto = _mapper.Map<TournamentDto>(tournamentDetails);

            // Apply the patch to the DTO, validating the model state
            patchDocument.ApplyTo(tournamentDto);

            // Validate the patched DTO if needed
            var validationContext = new ValidationContext(tournamentDto);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(tournamentDto, validationContext, validationResults, true))
            {
                throw new ValidationException("Validation failed for the patched DTO");
            }

            // Map the patched DTO back to the entity
            _mapper.Map(tournamentDto, tournamentDetails);

            // Update the repository
            _unitOfWork.TournamentRepository.Update(tournamentDetails);

            // Commit the changes to the database
            await _unitOfWork.CompleteAsync();

            return tournamentDto;
        }

        
    }
}
