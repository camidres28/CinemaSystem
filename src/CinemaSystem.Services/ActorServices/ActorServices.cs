using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Actors;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.StorageServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Services.ActorServices
{
    public class ActorServices : BaseServices, IActorServices
    {
        private readonly string container = "actors";
        private readonly IMapper mapper;
        private readonly ApplicationDbContext dbContext;
        private readonly IFileStorageServices fileStorage;

        public ActorServices(IMapper mapper, ApplicationDbContext dbContext,
            IFileStorageServices fileStorage)
            : base(dbContext, mapper)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
            this.fileStorage = fileStorage;
        }

        public async Task<ActorDto> CreateAsync(ActorCreateUpdateDto dto)
        {
            Actor entity = this.mapper.Map<Actor>(dto);
            if (dto.Photo != null)
            {
                using MemoryStream stream = new();
                await dto.Photo.CopyToAsync(stream);
                byte[] content = stream.ToArray();
                string extension = Path.GetExtension(dto.Photo.FileName);
                string uri = await this.fileStorage.SaveFileAsync(content, extension,
                    this.container, dto.Photo.ContentType);
                entity.PhotoUrl = uri;
            }

            await this.dbContext.Actors.AddAsync(entity);
            await this.dbContext.SaveChangesAsync();

            ActorDto actorDto = this.mapper.Map<ActorDto>(entity);

            return actorDto;
        }

        public async Task DeleteByIdAsync(int id)
        {
            await this.DeleteByIdAsync<Actor>(id);
        }

        public async Task<IEnumerable<ActorDto>> GetAllAsync(HttpContext httpContext, PaginationDto paginationDto)
        {
            IEnumerable<ActorDto> dtos = await this.GetAllAsync<Actor, ActorDto>(httpContext, paginationDto);

            return dtos;
        }

        public async Task<ActorDetailsDto> GetByIdAsync(int id)
        {
            Actor entity = await this.dbContext.Actors
                .Include(x => x.MoviesActors).ThenInclude(x => x.Movie)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (entity!= null)
            {
                entity.MoviesActors = entity.MoviesActors.OrderByDescending(x => x.Movie.ReleaseDate);
                ActorDetailsDto dto = this.mapper.Map<ActorDetailsDto>(entity);

                return dto;
            }
            
            return null;
        }

        public async Task UpdateAsync(int id, ActorCreateUpdateDto dto)
        {
            Actor entity = await this.dbContext.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity = this.mapper.Map(dto, entity);
                if (dto.Photo != null)
                {
                    using MemoryStream stream = new();
                    await dto.Photo.CopyToAsync(stream);
                    byte[] content = stream.ToArray();
                    string extension = Path.GetExtension(dto.Photo.FileName);
                    string uri = await this.fileStorage.EditFileAsync(content, entity.PhotoUrl, extension, this.container, dto.Photo.ContentType);
                    entity.PhotoUrl = uri;
                }
                //this.context.Entry(entity).State = EntityState.Modified;
                //this.context.Actors.Update(entity);
                await this.dbContext.SaveChangesAsync();
            }
        }
    }
}

