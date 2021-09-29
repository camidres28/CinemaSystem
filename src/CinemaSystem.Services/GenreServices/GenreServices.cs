using AutoMapper;
using CinemaSystem.Models.DTOs.Genres;
using CinemaSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.Services.GenreServices
{
    public class GenreServices : IGenreServices
    {
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;

        public GenreServices(IMapper mapper, ApplicationDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<GenreDto> CreateAsync(GenreCreateUpdateDto dto)
        {
            Genre entity = this.mapper.Map<Genre>(dto);
            this.context.Genres.Add(entity);
            await this.context.SaveChangesAsync();

            GenreDto genreDto = this.mapper.Map<GenreDto>(entity);

            return genreDto;
        }

        public async Task DeleteByIdAsync(int id)
        {
            Genre entity = await this.context.Genres.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                this.context.Genres.Remove(entity);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<GenreDto>> GetAllAsync()
        {
            IEnumerable<Genre> genres = await this.context.Genres.ToListAsync();
            IEnumerable<GenreDto> dtos = this.mapper.Map<IEnumerable<GenreDto>>(genres);

            return dtos;
        }

        public async Task<GenreDto> GetByIdAsync(int id)
        {
            Genre entity = await this.context.Genres.FirstOrDefaultAsync(x => x.Id == id);
            GenreDto dto = entity == null ? null : this.mapper.Map<GenreDto>(entity);

            return dto;
        }

        public async Task UpdateAsync(int id, GenreCreateUpdateDto dto)
        {
            bool exists = await this.context.Genres.AnyAsync(x => x.Id == id);
            if (exists)
            {
                Genre entity = this.mapper.Map<Genre>(dto);
                entity.Id = id;
                this.context.Entry(entity).State = EntityState.Modified;
                //this.context.Genres.Update(entity);
                await this.context.SaveChangesAsync();
            }            
        }
    }
}
