using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Movies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaSystem.Services.MovieServices
{
    public interface IMovieServices
    {
        Task<bool> DeleteByIdAsync(int id);
        Task<bool> ExistsMovieAsync(int movieId);
        Task<MovieDetailsDto> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, MovieCreateUpdateDto dto);
        Task<MovieDto> CreateAsync(MovieCreateUpdateDto dto);
        Task<IEnumerable<MovieDto>> GetAllAsync(HttpContext httpContext, PaginationDto paginationDto);
        Task<IEnumerable<MovieDto>> GetByFilteringAsync(HttpContext httpContext, FilterMoviesDto dto);
    }
}
