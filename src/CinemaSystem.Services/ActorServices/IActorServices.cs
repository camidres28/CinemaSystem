using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Actors;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Services.ActorServices
{
    public interface IActorServices
    {
        Task<ActorDto> CreateAsync(ActorCreateUpdateDto dto);
        Task<IEnumerable<ActorDto>> GetAllAsync(HttpContext httpContext, PaginationDto paginationDto);
        Task<ActorDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, ActorCreateUpdateDto dto);
        Task DeleteByIdAsync(int id);
    }
}
