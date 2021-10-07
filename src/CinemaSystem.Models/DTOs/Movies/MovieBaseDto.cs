using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.DTOs.Movies
{
    public class MovieBaseDto
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool IsOnCinema { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
    }
}
