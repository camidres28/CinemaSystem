using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        public MoviesGenres[] MoviesGenres { get; set; }
    }
}
