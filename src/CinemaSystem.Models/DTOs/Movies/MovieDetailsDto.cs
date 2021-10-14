using CinemaSystem.Models.DTOs.Genres;
using System.Collections.Generic;

namespace CinemaSystem.Models.DTOs.Movies
{
    public class MovieDetailsDto : MovieDto
    {
        public IEnumerable<GenreDto> Genres { get; set; }
        public IEnumerable<MovieActorDetailsDto> Actors { get; set; }
    }
}
