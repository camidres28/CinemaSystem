using CinemaSystem.Models.Validations;
using Microsoft.AspNetCore.Http;

namespace CinemaSystem.Models.DTOs.Actors
{
    public class ActorCreateUpdateDto : ActorBaseDto
    {
        [FileWeightValidation(maxWeightMegaBytes:4)]
        [FileTypeValidation(fileType:FileTypes.Image)]
        public IFormFile Photo { get; set; }
    }
}
