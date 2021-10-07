using CinemaSystem.Models.Validations;
using Microsoft.AspNetCore.Http;

namespace CinemaSystem.Models.DTOs.Movies
{
    public class MovieCreateUpdateDto : MovieBaseDto
    {
        [FileWeightValidation(maxWeightMegaBytes: 4)]
        [FileTypeValidation(fileType: FileTypes.Image)]
        public IFormFile Poster { get; set; }
    }
}
