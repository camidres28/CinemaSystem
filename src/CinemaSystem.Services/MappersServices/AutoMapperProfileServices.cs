using AutoMapper;
using CinemaSystem.Models.DTOs.Actors;
using CinemaSystem.Models.DTOs.Cinemas;
using CinemaSystem.Models.DTOs.Genres;
using CinemaSystem.Models.DTOs.Movies;
using CinemaSystem.Models.Entities;
using NetTopologySuite.Geometries;
using System.Collections.Generic;

namespace CinemaSystem.Services.MappersServices
{
    public class AutoMapperProfileServices : Profile
    {
        public AutoMapperProfileServices(GeometryFactory geometryFactory)
        {
            //Genres
            CreateMap<GenreDto, Genre>().ReverseMap();
            CreateMap<GenreCreateUpdateDto, Genre>().ReverseMap();

            //Actors
            CreateMap<Actor, ActorDto>().ReverseMap();
            CreateMap<ActorCreateUpdateDto, Actor>()
                .ForMember(x => x.PhotoUrl, options => options.Ignore());
            CreateMap<Actor, ActorDetailsDto>()
                .ForMember(x => x.Movies, options => options.MapFrom(this.MoviesActorsMapping));
            CreateMap<Actor, ActorBaseDto>().ReverseMap();

            //Movies
            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<MovieCreateUpdateDto, Movie>()
                .ForMember(x => x.PosterUrl, options => options.Ignore())
                .ForMember(x => x.MoviesActors, options => options.MapFrom(this.MoviesActorsMapping))
                .ForMember(x => x.MoviesGenres, options => options.MapFrom(this.MoviesGenresMapping));

            CreateMap<Movie, MovieDetailsDto>()
                .ForMember(x => x.Genres, options => options.MapFrom(this.MoviesGenresMapping))
                .ForMember(x => x.Actors, options => options.MapFrom(this.MoviesActorsMapping));

            CreateMap<Movie, MovieBaseDto>().ReverseMap();

            //Cinemas
            CreateMap<Cinema, CinemaDto>().ReverseMap();
            CreateMap<Cinema, CinemaDto>()
                .ForMember(x => x.Latitude, x => x.MapFrom(y => y.Location.Y))
                .ForMember(x => x.Longitude, x => x.MapFrom(y => y.Location.X));
            CreateMap<CinemaDto, Cinema>()
                .ForMember(x => x.Location, x => x.MapFrom(x =>
                   geometryFactory.CreatePoint(new Coordinate(x.Longitude, x.Latitude))));
            CreateMap<Cinema, CinemaDetailsDto>()
                .ForMember(x => x.Movies, options => options.MapFrom(this.MoviesCinemasMapping))
                .ForMember(x => x.Latitude, x => x.MapFrom(y => y.Location.Y))
                .ForMember(x => x.Longitude, x => x.MapFrom(y => y.Location.X));
            CreateMap<CinemaCreationUpdateDto, Cinema>()
                .ForMember(x => x.Location, x => x.MapFrom(x =>
                   geometryFactory.CreatePoint(new Coordinate(x.Longitude, x.Latitude))))
                .ForMember(x => x.MoviesCinemas, options => options.MapFrom(this.MoviesCinemasMapping));
        }

        private object Mapping(Actor arg)
        {
            throw new System.NotImplementedException();
        }

        private IEnumerable<GenreDto> MoviesGenresMapping(Movie movie, MovieDetailsDto dto)
        {
            List<GenreDto> result = new();
            if (movie.MoviesGenres == null)
            {
                return result;
            }

            foreach (MoviesGenres item in movie.MoviesGenres)
            {
                GenreDto genreDto = new()
                {
                    Id = item.GenreId,
                    Name = item.Genre.Name
                };

                result.Add(genreDto);
            }

            return result;
        }

        private IEnumerable<MovieActorDetailsDto> MoviesActorsMapping(Movie movie, MovieDetailsDto dto)
        {
            List<MovieActorDetailsDto> result = new();
            if (movie.MoviesActors == null)
            {
                return result;
            }

            foreach (MoviesActors item in movie.MoviesActors)
            {
                MovieActorDetailsDto ma = new()
                {
                    ActorId = item.ActorId,
                    Name = item.Actor.Name,
                    Character = item.Character
                };

                result.Add(ma);
            }

            return result;
        }

        private IEnumerable<MovieDto> MoviesActorsMapping(Actor actor, ActorDetailsDto dto)
        {
            List<MovieDto> result = new();
            if (actor.MoviesActors == null)
            {
                return result;
            }

            foreach (MoviesActors item in actor.MoviesActors)
            {
                MovieDto m = new()
                {
                    Id = item.MovieId,
                    Title = item.Movie.Title,
                    IsOnCinema = item.Movie.IsOnCinema,
                    ReleaseDate = item.Movie.ReleaseDate,
                    PosterUrl = item.Movie.PosterUrl
                };

                result.Add(m);
            }

            return result;
        }

        private IEnumerable<MoviesActors> MoviesActorsMapping(MovieCreateUpdateDto dto, Movie movie)
        {
            List<MoviesActors> result = new();

            if (dto.Actors == null)
            {
                return result;
            }

            foreach (var item in dto.Actors)
            {
                MoviesActors movieActor = new()
                {
                    ActorId = item.ActorId,
                    Character = item.Character
                };

                result.Add(movieActor);
            }


            return result;
        }

        private IEnumerable<MoviesGenres> MoviesGenresMapping(MovieCreateUpdateDto dto, Movie movie)
        {
            List<MoviesGenres> result = new();

            if (dto.GenresIds == null)
            {
                return result;
            }

            foreach (var item in dto.GenresIds)
            {
                MoviesGenres movieGenre = new()
                {
                    GenreId = item
                };

                result.Add(movieGenre);
            }

            return result;
        }

        private IEnumerable<MovieDto> MoviesCinemasMapping(Cinema entity, CinemaDetailsDto dto)
        {
            List<MovieDto> result = new();

            if (entity.MoviesCinemas == null)
            {
                return result;
            }

            foreach (MoviesCinemas item in entity.MoviesCinemas)
            {
                MovieDto movieDto = new()
                {
                    Id = item.MovieId,
                    Title = item.Movie.Title,
                    IsOnCinema = item.Movie.IsOnCinema,
                    ReleaseDate = item.Movie.ReleaseDate,
                    PosterUrl = item.Movie.PosterUrl
                };

                result.Add(movieDto);
            }

            return result;
        }

        private IEnumerable<MoviesCinemas> MoviesCinemasMapping(CinemaCreationUpdateDto dto, Cinema entity)
        {
            List<MoviesCinemas> result = new();
            if (dto.MoviesId == null)
            {
                return result;
            }

            foreach (int item in dto.MoviesId)
            {
                MoviesCinemas mc = new() { MovieId = item };

                result.Add(mc);
            }

            return result;
        }
    }
}
