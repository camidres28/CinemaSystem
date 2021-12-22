using AutoMapper;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.MappersServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.Security.Claims;

namespace CinemaSystem.UnitTests.ServicesTests
{
    public class BaseServicesTests
    {
        protected string usuarioPorDefectoId = "9722b56a-77ea-4e41-941d-e319b6eb3712";
        protected string usuarioPorDefectoEmail = "ejemplo@email.com";

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

        protected HttpContext BuildHttpContextWithClaims()
        {
            ClaimsPrincipal user = new(
                new ClaimsIdentity(
                    new Claim[]
                    {
                        new(ClaimTypes.Name, this.usuarioPorDefectoEmail),
                        new(ClaimTypes.Email, this.usuarioPorDefectoEmail),
                        new(ClaimTypes.NameIdentifier, this.usuarioPorDefectoId)
                    }));

            HttpContext httpContext = new DefaultHttpContext()
            {
                User = user
            };

            return httpContext;
        }
    }
}
