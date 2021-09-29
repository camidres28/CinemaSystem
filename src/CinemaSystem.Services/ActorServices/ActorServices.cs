using AutoMapper;
using CinemaSystem.Models.DTOs.Actors;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.StorageServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.Services.ActorServices
{
    public class ActorServices : IActorServices
    {
        private readonly string container = "actors";
        private readonly IMapper mapper;
        private readonly ApplicationDbContext context;
        private readonly IFileStorageServices fileStorage;

        public ActorServices(IMapper mapper, ApplicationDbContext context, 
            IFileStorageServices fileStorage)
        {
            this.mapper = mapper;
            this.context = context;
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
                string uri = await this.fileStorage.SaveFile(content, extension, this.container, dto.Photo.ContentType);
                entity.PhotoUrl = uri;
            }

            await this.context.Actors.AddAsync(entity);
            await this.context.SaveChangesAsync();

            ActorDto actorDto = this.mapper.Map<ActorDto>(entity);

            return actorDto;
        }

        public async Task DeleteByIdAsync(int id)
        {
            Actor entity = await this.context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                this.context.Actors.Remove(entity);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ActorDto>> GetAllAsync()
        {
            IEnumerable<Actor> entities = await this.context.Actors.ToListAsync();
            IEnumerable<ActorDto> dtos = this.mapper.Map<IEnumerable<ActorDto>>(entities);

            return dtos;
        }

        public async Task<ActorDto> GetByIdAsync(int id)
        {
            Actor entity = await this.context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                ActorDto dto = this.mapper.Map<ActorDto>(entity);
                return dto;
            }

            return null;
        }

        public async Task UpdateAsync(int id, ActorCreateUpdateDto dto)
        {
            Actor entity = await this.context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity = this.mapper.Map(dto, entity);
                if (dto.Photo != null)
                {
                    using MemoryStream stream = new();
                    await dto.Photo.CopyToAsync(stream);
                    byte[] content = stream.ToArray();
                    string extension = Path.GetExtension(dto.Photo.FileName);
                    string uri = await this.fileStorage.EditFile(content, entity.PhotoUrl, extension, this.container, dto.Photo.ContentType);
                    entity.PhotoUrl = uri;
                }
                //this.context.Entry(entity).State = EntityState.Modified;
                //this.context.Actors.Update(entity);
                await this.context.SaveChangesAsync();
            }
        }
    }
}
