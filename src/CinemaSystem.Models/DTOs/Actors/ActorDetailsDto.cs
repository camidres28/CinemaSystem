using CinemaSystem.Models.DTOs.Movies;
using System.Collections.Generic;

namespace CinemaSystem.Models.DTOs.Actors
{
    public class ActorDetailsDto : ActorDto
    {
        public IEnumerable<MovieDto> Movies { get; set; }
    }
}
