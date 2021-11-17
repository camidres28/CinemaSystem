using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Actors;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaSystem.Services.ActorServices
{
    public interface IActorServices
    {
        Task<ActorDto> CreateAsync(ActorCreateUpdateDto dto);
        Task<IEnumerable<ActorDto>> GetAllAsync(HttpContext httpContext, PaginationDto paginationDto);
        Task<ActorDetailsDto> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, ActorCreateUpdateDto dto);
        Task<bool> DeleteByIdAsync(int id);
    }
}
