using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Movies;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.ExtensionsServices;
using CinemaSystem.Services.StorageServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace CinemaSystem.Services.MovieServices
{
    public class MovieServices : BaseServices, IMovieServices
    {
        private readonly string container = "movies";
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;
        private readonly IFileStorageServices fileStorage;
        private readonly ILogger<MovieServices> logger;

        public MovieServices(IMapper mapper, ApplicationDbContext dbContext,
            IFileStorageServices fileStorage,
            ILogger<MovieServices> logger)
            : base(dbContext, mapper)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.fileStorage = fileStorage;
            this.logger = logger;
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

            this.AsigningOrderToActors(entity);
            await this.dbContext.Movies.AddAsync(entity);
            await this.dbContext.SaveChangesAsync();

            MovieDto movieDto = this.mapper.Map<MovieDto>(entity);

            return movieDto;
        }

        public async Task DeleteByIdAsync(int id)
        {
            await this.DeleteByIdAsync<Movie>(id);
        }

        public async Task<IEnumerable<MovieDto>> GetAllAsync(HttpContext httpContext, PaginationDto paginationDto)
        {
            IEnumerable<MovieDto> dtos = await this.GetAllAsync<Movie, MovieDto>(httpContext, paginationDto);

            return dtos;
        }

        public async Task<IEnumerable<MovieDto>> GetByFilteringAsync(HttpContext httpContext, FilterMoviesDto dto)
        {
            IQueryable<Movie> queryable = this.dbContext.Movies.AsQueryable();
            if (!string.IsNullOrWhiteSpace(dto.Title))
            {
                queryable = queryable.Where(x => x.Title.Contains(dto.Title));
            }

            if (dto.OnCinema)
            {
                queryable = queryable.Where(x => x.IsOnCinema);
            }

            if (dto.FutureReleases)
            {
                DateTimeOffset today = new DateTimeOffset(DateTime.Now);
                queryable = queryable.Where(x => x.ReleaseDate > today);
            }

            if (dto.GenreId > 0)
            {
                queryable = queryable.Where(x => x.MoviesGenres.Select(x => x.GenreId).Contains(dto.GenreId));
            }

            if (!string.IsNullOrWhiteSpace(dto.OrderField))
            {
                string orderType = dto.OrderAscending ? "ascending" : "descending";
                try
                {
                    queryable = queryable.OrderBy($"{dto.OrderField} {orderType}");
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex.Message, ex);
                }
            }

            await httpContext.InsertPaginationParameters(queryable, dto.RegistersPerPageQuantity);

            IEnumerable<Movie> movies = await queryable.Paginate(dto).ToListAsync();
            IEnumerable<MovieDto> movieDtos = this.mapper.Map<IEnumerable<MovieDto>>(movies);

            return movieDtos;
        }

        public async Task<MovieDetailsDto> GetByIdAsync(int id)
        {
            MovieDetailsDto dto = null;
            Movie entity = await this.dbContext.Movies
                .Include(x => x.MoviesActors).ThenInclude(x => x.Actor)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity.MoviesActors = entity.MoviesActors.OrderBy(x => x.Order).ToList();
                dto = this.mapper.Map<MovieDetailsDto>(entity);
            }

            return dto;
        }

        public async Task UpdateAsync(int id, MovieCreateUpdateDto dto)
        {
            Movie entity = await this.dbContext.Movies
                .Include(x => x.MoviesActors)
                .Include(x => x.MoviesGenres)
                .FirstOrDefaultAsync(x => x.Id == id);
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

                this.AsigningOrderToActors(entity);
                this.dbContext.Entry(entity).State = EntityState.Modified;
                await this.dbContext.SaveChangesAsync();
            }
        }


        private void AsigningOrderToActors(Movie movie)
        {
            if (movie.MoviesActors != null)
            {
                int order = 1;
                foreach (MoviesActors ma in movie.MoviesActors)
                {
                    ma.Order = order;
                    order++;
                }
            }
        }
    }
}
