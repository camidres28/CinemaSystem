using AutoMapper;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.MappersServices;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace CinemaSystem.UnitTests.ServicesTests
{
    public class BaseServicesTests
    {
        public GeometryFactory GeometryFactory { get => NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326); }
        protected ApplicationDbContext BuildContext(string dbName)
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName).Options;

            ApplicationDbContext dbContext = new(options);

            return dbContext;
        }

        protected IMapper SeetingAutoMapper()
        {
            var config = new MapperConfiguration(options => 
            {
                options.AddProfile(new AutoMapperProfileServices(this.GeometryFactory));
            });

            return config.CreateMapper();
        }
    }
}
