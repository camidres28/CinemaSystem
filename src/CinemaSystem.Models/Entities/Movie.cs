using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
