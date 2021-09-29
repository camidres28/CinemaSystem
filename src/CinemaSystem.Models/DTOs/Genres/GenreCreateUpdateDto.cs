using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.DTOs.Genres
{
    public class GenreCreateUpdateDto
    {
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
    }
}
