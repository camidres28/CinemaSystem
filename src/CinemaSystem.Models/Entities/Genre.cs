using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.Entities
{
    public class Genre : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        public IEnumerable<MoviesGenres> MoviesGenres { get; set; }
    }
}
