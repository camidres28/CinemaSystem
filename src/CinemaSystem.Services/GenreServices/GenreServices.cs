using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Genres;
using CinemaSystem.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaSystem.Services.GenreServices
{
    public class GenreServices : BaseServices, IGenreServices
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;

        public GenreServices(IMapper mapper, ApplicationDbContext dbContext)
            :base(dbContext, mapper)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public async Task<GenreDto> CreateAsync(GenreCreateUpdateDto dto)
        {
            Genre entity = this.mapper.Map<Genre>(dto);
            this.dbContext.Genres.Add(entity);
            await this.dbContext.SaveChangesAsync();

            GenreDto genreDto = this.mapper.Map<GenreDto>(entity);

            return genreDto;
        }

        public async Task DeleteByIdAsync(int id)
        {
            await this.DeleteByIdAsync<Genre>(id);            
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

        public async Task UpdateAsync(int id, GenreCreateUpdateDto dto)
        {
            bool exists = await this.dbContext.Genres.AnyAsync(x => x.Id == id);
            if (exists)
            {
                Genre entity = this.mapper.Map<Genre>(dto);
                entity.Id = id;
                this.dbContext.Entry(entity).State = EntityState.Modified;
                await this.dbContext.SaveChangesAsync();
            }            
        }
    }
}
