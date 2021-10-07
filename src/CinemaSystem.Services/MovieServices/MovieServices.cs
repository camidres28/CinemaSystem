using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Movies;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.ExtensionsServices;
using CinemaSystem.Services.StorageServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.Services.MovieServices
{
    public class MovieServices : IMovieServices
    {
        private readonly string container = "movies";
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;
        private readonly IFileStorageServices fileStorage;

        public MovieServices(IMapper mapper, ApplicationDbContext dbContext,
            IFileStorageServices fileStorage)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.fileStorage = fileStorage;
        }

        public async Task<MovieDto> CreateAsync(MovieCreateUpdateDto dto)
        {
            Movie entity = this.mapper.Map<Movie>(dto);
            if (dto.Poster != null)
            {
                using MemoryStream stream = new();
                await dto.Poster.CopyToAsync(stream);
                byte[] content = stream.ToArray();
                string extension = Path.GetExtension(dto.Poster.FileName);
                string url = await this.fileStorage.SaveFileAsync(content, extension,
                    this.container, dto.Poster.ContentType);
                entity.PosterUrl = url;
            }

            await this.dbContext.Movies.AddAsync(entity);
            await this.dbContext.SaveChangesAsync();

            MovieDto movieDto = this.mapper.Map<MovieDto>(entity);

            return movieDto;
        }

        public async Task DeleteByIdAsync(int id)
        {
            bool exists = await this.dbContext.Movies.AnyAsync(x => x.Id == id);
            if (exists)
            {
                this.dbContext.Movies.Remove(new Movie { Id = id });
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MovieDto>> GetAllAsync(HttpContext httpContext, PaginationDto paginationDto)
        {
            IQueryable<Movie> queryable = this.dbContext.Movies.AsQueryable();
            await httpContext.InsertPaginationParameters(queryable, paginationDto.RegistersPerPageQuantity);
            IEnumerable<Movie> entities = await queryable.Paginate(paginationDto).ToListAsync();
            IEnumerable<MovieDto> dtos = this.mapper.Map<IEnumerable<MovieDto>>(entities);

            return dtos;
        }

        public async Task<MovieDto> GetByIdAsync(int id)
        {
            Movie entity = await this.dbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                MovieDto dto = this.mapper.Map<MovieDto>(entity);
                return dto;
            }

            return null;
        }

        public async Task UpdateAsync(int id, MovieCreateUpdateDto dto)
        {
            Movie entity = await this.dbContext.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity = this.mapper.Map(dto, entity);
                if (dto.Poster != null)
                {
                    using MemoryStream stream = new();
                    await dto.Poster.CopyToAsync(stream);
                    byte[] content = stream.ToArray();
                    string extension = Path.GetExtension(dto.Poster.FileName);
                    string url = await this.fileStorage.EditFileAsync(content,
                        entity.PosterUrl, extension, this.container, dto.Poster.ContentType);
                    entity.PosterUrl = url;
                }

                this.dbContext.Entry(entity).State = EntityState.Modified;
                await this.dbContext.SaveChangesAsync();
            }
        }
    }
}
