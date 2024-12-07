using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.DTO;
using Tournament.Core.DTOs;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public class TournamentMappings: Profile
    {
        public TournamentMappings()
        {
            CreateMap<TournamentDetails,TournamentDto>()
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.StartDate.AddMonths(3)))
              //  .ForMember(dest => dest.Games, opt => opt.MapFrom(src => src.Games))

                .ReverseMap();

            CreateMap<Game,GameDto>().ReverseMap();
            CreateMap<TournamentDetails, TournamentCreateDto>().ReverseMap();
        }

    }
}
