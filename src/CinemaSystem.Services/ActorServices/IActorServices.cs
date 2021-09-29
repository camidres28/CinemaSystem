using CinemaSystem.Models.DTOs.Actors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaSystem.Services.ActorServices
{
    public interface IActorServices
    {
        Task<ActorDto> CreateAsync(ActorCreateUpdateDto dto);
        Task<IEnumerable<ActorDto>> GetAllAsync();
        Task<ActorDto> GetByIdAsync(int id);
        Task UpdateAsync(int id, ActorCreateUpdateDto dto);
        Task DeleteByIdAsync(int id);
    }
}
