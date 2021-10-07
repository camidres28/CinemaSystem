using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Movies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.Services.MovieServices
{
    public interface IMovieServices
    {
        Task<MovieDto> CreateAsync(MovieCreateUpdateDto dto);
        Task<IEnumerable<MovieDto>> GetAllAsync(HttpContext httpContext, PaginationDto paginationDto);
        Task<MovieDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, MovieCreateUpdateDto dto);
        Task DeleteByIdAsync(int id);
    }
}
