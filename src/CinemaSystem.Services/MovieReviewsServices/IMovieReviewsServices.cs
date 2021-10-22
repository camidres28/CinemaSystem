using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Reviews;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaSystem.Services.MovieReviewsServices
{
    public interface IMovieReviewsServices
    {
        Task<IEnumerable<ReviewReadingDto>> GetAllAsync(int movieId,
            HttpContext httpContext,
            PaginationDto paginationDto);
        Task<ReviewReadingDto> CreateAsync(int movieId,
            HttpContext context,
            ReviewCreateUpdateDto dto);
        Task<ReviewReadingDto> GetByIdAsync(int reviewId);
        Task<bool> UpdateAsync(int id, HttpContext context, ReviewCreateUpdateDto dto);
        Task<bool> DeleteByIdAsync(int id, HttpContext context);
    }
}
