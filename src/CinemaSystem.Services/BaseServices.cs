using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.ExtensionsServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Services
{
    public class BaseServices
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public BaseServices(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        protected async Task<IEnumerable<TDTO>> GetAllAsync<TEntity, TDTO>(HttpContext httpContext,
            PaginationDto paginationDto) where TEntity : class
        {
            IQueryable<TEntity> queryable = this.dbContext.Set<TEntity>().AsQueryable<TEntity>();
            IEnumerable<TDTO> dtos = await this.GetAllAsync<TEntity,TDTO>(httpContext, paginationDto, queryable);

            return dtos;
        }

        protected async Task<IEnumerable<TDTO>> GetAllAsync<TEntity, TDTO>(HttpContext httpContext,
            PaginationDto paginationDto, IQueryable<TEntity> queryable) where TEntity : class
        {
            await httpContext.InsertPaginationParameters(queryable, paginationDto.RegistersPerPageQuantity);
            IEnumerable<TEntity> genres = await queryable.Paginate(paginationDto).ToListAsync();
            IEnumerable<TDTO> dtos = this.mapper.Map<IEnumerable<TDTO>>(genres);

            return dtos;
        }

        protected async Task<TDTO> GetByIdAsync<TEntity, TDTO>(int id)
            where TEntity : class, IId
        {
            TEntity entity = await this.dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            TDTO dto = this.mapper.Map<TDTO>(entity);

            return dto;
        }

        protected async Task<bool> DeleteByIdAsync<TEntity>(int id) where TEntity : class, IId, new()
        {
            bool result = false;
            bool exists = await this.dbContext.Set<TEntity>().AnyAsync(x => x.Id == id);
            if (exists)
            {
                this.dbContext.Remove(new TEntity() { Id = id });
                await this.dbContext.SaveChangesAsync();
                result = true; 
            }

            return result;
        }

        protected async Task<TReadingDto> CreateAsync<TEntity, TReadingDto, TCreation>(TCreation dto)
            where TEntity : class
        {
            TEntity entity = this.mapper.Map<TEntity>(dto);
            await this.dbContext.Set<TEntity>().AddAsync(entity);
            await this.dbContext.SaveChangesAsync();

            TReadingDto readingDto = this.mapper.Map<TReadingDto>(entity);

            return readingDto;
        }

        protected async Task<bool> UpdateAsync<TEntity, TUpdate>(int id, TUpdate dto)
            where TEntity:class, IId
        {
            bool result = false;
            bool exists = await this.dbContext.Set<TEntity>().AnyAsync(x => x.Id == id);
            if (exists)
            {
                TEntity entity = this.mapper.Map<TEntity>(dto);
                entity.Id = id;
                this.dbContext.Entry(entity).State = EntityState.Modified;
                await this.dbContext.SaveChangesAsync();
                result = true;
            }

            return result;
        }
    }
}
