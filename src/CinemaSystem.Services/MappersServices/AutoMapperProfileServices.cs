using AutoMapper;
using CinemaSystem.Models.DTOs.Actors;
using CinemaSystem.Models.DTOs.Genres;
using CinemaSystem.Models.Entities;

namespace CinemaSystem.Services.MappersServices
{
    public class AutoMapperProfileServices : Profile
    {
        public AutoMapperProfileServices()
        {
            CreateMap<GenreDto, Genre>().ReverseMap();
            CreateMap<GenreCreateUpdateDto, Genre>();

            CreateMap<Actor, ActorDto>().ReverseMap();
            CreateMap<ActorCreateUpdateDto, Actor>()
                .ForMember(x=>x.PhotoUrl, options=>options.Ignore());
        }
    }
}
