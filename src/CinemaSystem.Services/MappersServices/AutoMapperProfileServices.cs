using AutoMapper;
using CinemaSystem.Models.DTOs.Actors;
using CinemaSystem.Models.DTOs.Genres;
using CinemaSystem.Models.DTOs.Movies;
using CinemaSystem.Models.Entities;

namespace CinemaSystem.Services.MappersServices
{
    public class AutoMapperProfileServices : Profile
    {
        public AutoMapperProfileServices()
        {
            //Genres
            CreateMap<GenreDto, Genre>().ReverseMap();
            CreateMap<GenreCreateUpdateDto, Genre>();

            //Actors
            CreateMap<Actor, ActorDto>().ReverseMap();
            CreateMap<ActorCreateUpdateDto, Actor>()
                .ForMember(x=>x.PhotoUrl, options=>options.Ignore());
            CreateMap<Actor, ActorBaseDto>().ReverseMap();

            //Movies
            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<MovieCreateUpdateDto, Movie>()
                .ForMember(x => x.PosterUrl, options => options.Ignore());
            CreateMap<Movie, MovieBaseDto>().ReverseMap();
        }
    }
}
