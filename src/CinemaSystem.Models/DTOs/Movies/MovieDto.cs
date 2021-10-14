using CinemaSystem.Models.Entities;

namespace CinemaSystem.Models.DTOs.Movies
{
    public class MovieDto : MovieBaseDto
    {
        public int Id { get; set; }
        public string PosterUrl { get; set; }
    }
}
