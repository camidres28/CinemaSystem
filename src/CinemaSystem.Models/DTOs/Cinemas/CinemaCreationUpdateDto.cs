using System.Collections.Generic;

namespace CinemaSystem.Models.DTOs.Cinemas
{
    public class CinemaCreationUpdateDto: CinemaBaseDto
    {
        public IEnumerable<int> MoviesId { get; set; }
    }
}
