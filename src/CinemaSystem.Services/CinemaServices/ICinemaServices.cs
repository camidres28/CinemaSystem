using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Cinemas;
using CinemaSystem.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaSystem.Services.CinemaServices
{
    public interface ICinemaServices
    {
        Task DeleteByIdAsync(int id);
        Task<CinemaDetailsDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, CinemaCreationUpdateDto dto);
        Task<CinemaDto> CreateAsync(CinemaCreationUpdateDto dto);
        Task<IEnumerable<CinemaNearbyDto>> GetNearby(CinemaNearbyFilterDto dto);
        Task<IEnumerable<CinemaDto>> GetAllAsync(HttpContext httpContext, PaginationDto paginationDto);
    }
}