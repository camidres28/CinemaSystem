using CinemaSystem.Models.DTOs.Movies;
using System.Collections.Generic;

namespace CinemaSystem.Models.DTOs.Cinemas
{
    public class CinemaDetailsDto : CinemaDto
    {
        public IEnumerable<MovieDto> Movies { get; set; }
    }
}
