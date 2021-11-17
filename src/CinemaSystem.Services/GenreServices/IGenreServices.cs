using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Genres;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaSystem.Services.GenreServices
{
    public interface IGenreServices
    {
        Task<GenreDto> CreateAsync(GenreCreateUpdateDto dto);
        Task<IEnumerable<GenreDto>> GetAllAsync(HttpContext httpContext, PaginationDto paginationDto);
        Task<GenreDto> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, GenreCreateUpdateDto dto);
        Task<bool> DeleteByIdAsync(int id);
    }
}
