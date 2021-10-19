using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.Entities
{
    public class Movie : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool IsOnCinema { get; set; }
        public string PosterUrl { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public IEnumerable<MoviesActors> MoviesActors { get; set; }
        public IEnumerable<MoviesGenres> MoviesGenres { get; set; }
        public IEnumerable<MoviesCinemas> MoviesCinemas { get; set; }
    }
}
