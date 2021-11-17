using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Genres;
using CinemaSystem.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaSystem.Services.GenreServices
{
    public class GenreServices : BaseServices, IGenreServices
    {
        public GenreServices(IMapper mapper, ApplicationDbContext dbContext)
            : base(dbContext, mapper)
        {

        }

        public async Task<GenreDto> CreateAsync(GenreCreateUpdateDto dto)
        {
            GenreDto genreDto = await this.CreateAsync<Genre, GenreDto, GenreCreateUpdateDto>(dto);

            return genreDto;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            return await this.DeleteByIdAsync<Genre>(id);
        }

        public async Task<IEnumerable<GenreDto>> GetAllAsync(HttpContext httpContext, PaginationDto paginationDto)
        {
            IEnumerable<GenreDto> dtos = await this.GetAllAsync<Genre, GenreDto>(httpContext, paginationDto);

            return dtos;
        }

        public async Task<GenreDto> GetByIdAsync(int id)
        {
            GenreDto dto = await this.GetByIdAsync<Genre, GenreDto>(id);

            return dto;
        }

        public async Task<bool> UpdateAsync(int id, GenreCreateUpdateDto dto)
        {
            return await this.UpdateAsync<Genre, GenreCreateUpdateDto>(id, dto);
        }
    }
}
