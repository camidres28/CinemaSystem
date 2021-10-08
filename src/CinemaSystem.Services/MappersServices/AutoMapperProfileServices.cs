using AutoMapper;
using CinemaSystem.Models.DTOs.Actors;
using CinemaSystem.Models.DTOs.Genres;
using CinemaSystem.Models.DTOs.Movies;
using CinemaSystem.Models.DTOs.MoviesActors;
using CinemaSystem.Models.Entities;
using System.Collections.Generic;

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
                .ForMember(x => x.PosterUrl, options => options.Ignore())
                .ForMember(x=>x.MoviesActors, options => options.MapFrom(this.MoviesActorsMapping))
                .ForMember(x=>x.MoviesGenres, options => options.MapFrom(this.MoviesGenresMapping));
            CreateMap<Movie, MovieBaseDto>().ReverseMap();

        }

        private MoviesActors[] MoviesActorsMapping(MovieCreateUpdateDto dto, Movie movie)
        {
            List<MoviesActors> result = new();

            if (dto.Actors == null)
            {
                return result.ToArray();
            }

            int l = dto.Actors.Length;
            for (int i = 0; i < l; i++)
            {
                MoviesActorsCreationUpdateDto movieActorDto = dto.Actors[i];
                MoviesActors movieActor = new()
                {
                    ActorId = movieActorDto.ActorId,
                    Character = movieActorDto.Character
                };

                result.Add(movieActor);
            }

            return result.ToArray();
        }

        private MoviesGenres[] MoviesGenresMapping(MovieCreateUpdateDto dto, Movie movie)
        {
            List<MoviesGenres> result = new();

            if (dto.GenresIds == null)
            {
                return result.ToArray();
            }

            int l = dto.GenresIds.Length;
            for (int i = 0; i < l; i++)
            {
                int genreId = dto.GenresIds[i];
                MoviesGenres movieGenre = new()
                {
                    GenreId = genreId
                };

                result.Add(movieGenre);
            }

            return result.ToArray();
        }
    }
}
