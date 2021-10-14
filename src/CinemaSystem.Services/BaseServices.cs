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

        protected async Task DeleteByIdAsync<TEntity>(int id) where TEntity : class, IId, new()
        {
            bool exists = await this.dbContext.Set<TEntity>().AnyAsync(x => x.Id == id);
            if (exists)
            {
                this.dbContext.Remove(new TEntity() { Id = id });
                await this.dbContext.SaveChangesAsync();
            }
        }
    }
}
