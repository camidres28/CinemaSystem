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
                GeometryFactory geometry = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                options.AddProfile(new AutoMapperProfileServices(geometry));
            });

            return config.CreateMapper();
        }
    }
}
