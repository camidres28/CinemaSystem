using System.Collections.Generic;

namespace CinemaSystem.Models.DTOs.Movies
{
    public class MoviesIndexDto
    {
        public IEnumerable<MovieDto> FutureReleases { get; set; }
        public IEnumerable<MovieDto> OnCinemas { get; set; }
    }
}
