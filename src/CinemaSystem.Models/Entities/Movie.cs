using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool IsOnCinema { get; set; }
        public string PosterUrl { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public MoviesActors[] MoviesActors { get; set; }
        public MoviesGenres[] MoviesGenres { get; set; }
    }
}
