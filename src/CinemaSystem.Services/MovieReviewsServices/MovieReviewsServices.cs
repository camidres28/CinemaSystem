using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Reviews;
using CinemaSystem.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CinemaSystem.Services.MovieReviewsServices
{
    public class MovieReviewsServices : BaseServices, IMovieReviewsServices
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public MovieReviewsServices(ApplicationDbContext dbContext,
            IMapper mapper)
            : base(dbContext, mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }


        public async Task<IEnumerable<ReviewReadingDto>> GetAllAsync(int movieId,
            HttpContext httpContext,
            PaginationDto paginationDto)
        {
            IQueryable<Review> queryable = this.dbContext.Reviews
                .Include(x => x.User)
                .Where(x => x.MovieId == movieId)
                .AsQueryable();

            IEnumerable<ReviewReadingDto> dtos = await this.GetAllAsync<Review, ReviewReadingDto>(
                httpContext, paginationDto, queryable);

            return dtos;
        }

        public async Task<ReviewReadingDto> GetByIdAsync(int id)
        {
            ReviewReadingDto dto = null;
            Review entity = await this.dbContext.Reviews.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                dto = this.mapper.Map<ReviewReadingDto>(entity);
            }

            return dto;
        }

        public async Task<ReviewReadingDto> CreateAsync(int movieId,
            HttpContext context,
            ReviewCreateUpdateDto dto)
        {
            ReviewReadingDto reviewReadingDto = null;
            string userId = context.User.Claims.
                FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

            bool reviewExists = await this.dbContext.Reviews
                .AnyAsync(x => x.MovieId == movieId && x.UserId == userId);
            if (!reviewExists)
            {
                Review entity = this.mapper.Map<Review>(dto);
                entity.MovieId = movieId;
                entity.UserId = userId;
                await this.dbContext.Reviews.AddAsync(entity);
                await this.dbContext.SaveChangesAsync();

                entity = await this.dbContext.Reviews.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == entity.Id);
                reviewReadingDto = this.mapper.Map<ReviewReadingDto>(entity);
            }

            return reviewReadingDto;
        }

        public async Task<bool> UpdateAsync(int id,
            HttpContext context,
            ReviewCreateUpdateDto dto)
        {
            bool exists = await this.ExistsReview(id, context);
            if (!exists)
            {
                return false;
            }

            Review entity = await this.dbContext.Reviews.FirstOrDefaultAsync(x => x.Id == id);
            entity = this.mapper.Map(dto, entity);
            this.dbContext.Entry(entity).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteByIdAsync(int id, HttpContext context)
        {
            bool exists = await this.ExistsReview(id, context);
            if (!exists)
            {
                return false;
            }

            this.dbContext.Reviews.Remove(new Review() { Id = id });
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> ExistsReview(int id, HttpContext context)
        {
            string userId = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            bool exists = await this.dbContext.Reviews.AnyAsync(x => x.Id == id && x.UserId == userId);

            return exists;
        }
    }
}
