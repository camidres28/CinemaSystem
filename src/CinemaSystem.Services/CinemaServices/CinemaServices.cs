using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Cinemas;
using CinemaSystem.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.Services.CinemaServices
{
    public class CinemaServices : BaseServices, ICinemaServices
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        private readonly GeometryFactory geometryFactory;

        public CinemaServices(ApplicationDbContext dbContext,
            IMapper mapper,
            GeometryFactory geometryFactory)
            : base(dbContext, mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.geometryFactory = geometryFactory;
        }

        public async Task<CinemaDto> CreateAsync(CinemaCreationUpdateDto dto)
        {
            CinemaDto cinemaDto = await this.CreateAsync<Cinema, CinemaDto, CinemaCreationUpdateDto>(dto);

            return cinemaDto;
        }

        public async Task DeleteByIdAsync(int id)
        {
            await this.DeleteByIdAsync<Cinema>(id);
        }

        public async Task<IEnumerable<CinemaDto>> GetAllAsync(HttpContext httpContext, PaginationDto paginationDto)
        {
            IEnumerable<CinemaDto> dtos = await this.GetAllAsync<Cinema, CinemaDto>(httpContext, paginationDto);

            return dtos;
        }

        public async Task<CinemaDetailsDto> GetByIdAsync(int id)
        {
            CinemaDetailsDto dto = null;
            Cinema entity = await this.dbContext.Cinemas
                            .Include(x => x.MoviesCinemas).ThenInclude(x => x.Movie)
                            .FirstOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                dto = this.mapper.Map<CinemaDetailsDto>(entity);
            }

            return dto;
        }

        public async Task UpdateAsync(int id, CinemaCreationUpdateDto dto)
        {
            Cinema entity = await this.dbContext.Cinemas
                .Include(x => x.MoviesCinemas).ThenInclude(x => x.Movie)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                entity = this.mapper.Map(dto, entity);
                this.dbContext.Cinemas.Update(entity);
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CinemaNearbyDto>> GetNearby(CinemaNearbyFilterDto dto)
        {
            Point userLocation = this.geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude));
            IEnumerable<CinemaNearbyDto> dtos = await this.dbContext.Cinemas                
                .Where(x => x.Location.IsWithinDistance(userLocation, dto.DistanceKm * 1000))
                .OrderBy(x => x.Location.Distance(userLocation))
                .Select(x => new CinemaNearbyDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Longitude = x.Location.X,
                    Latitude = x.Location.Y,
                    DistanceKm = Math.Round(x.Location.Distance(userLocation) / 1000, 3)
                }).ToListAsync();

            return dtos;
        }
    }
}
