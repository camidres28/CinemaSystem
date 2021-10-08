using CinemaSystem.Models.DTOs.MoviesActors;
using CinemaSystem.Models.Helpers;
using CinemaSystem.Models.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaSystem.Models.DTOs.Movies
{
    public class MovieCreateUpdateDto : MovieBaseDto
    {
        [FileWeightValidation(maxWeightMegaBytes: 4)]
        [FileTypeValidation(fileType: FileTypes.Image)]
        public IFormFile Poster { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<int[]>))]
        public int[] GenresIds { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<MoviesActorsCreationUpdateDto[]>))]
        public MoviesActorsCreationUpdateDto[] Actors { get; set; }
    }
}
