using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Cinemas;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.CinemaServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.UnitTests.ServicesTests
{
    [TestClass]
    public class CinemaServicesTests : BaseServicesTests
    {
        [TestMethod]
        public async Task CreateCinemaWithoutMoviesTest()
        {
            //preparation
            string dbName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext1 = this.BuildContext(dbName);
            IMapper mapper = this.SeetingAutoMapper();

            ICinemaServices cinemaServices = new CinemaServices(dbContext1, mapper, this.GeometryFactory);

            //1.2055371680316427, -77.26046462525754
            CinemaCreationUpdateDto dto1 = new()
            {
                Name = "CC Unico Outlet - Pasto",
                Latitude = 1.2055371680316427,
                Longitude = -77.26046462525754
            };

            //1.2169715540261588, -77.28860043409382
            CinemaCreationUpdateDto dto2 = new()
            {
                Name = "CC Unicentro - Pasto",
                Latitude = 1.2169715540261588,
                Longitude = -77.28860043409382
            };

            //Execution:
            CinemaDto cinemaDto1 = await cinemaServices.CreateAsync(dto1);
            CinemaDto cinemaDto2 = await cinemaServices.CreateAsync(dto2);

            //Verification:
            Assert.IsNotNull(cinemaDto1);
            Assert.IsNotNull(cinemaDto2);
            Assert.AreEqual(dto1.Name, cinemaDto1.Name);
            Assert.AreEqual(dto1.Latitude, cinemaDto1.Latitude);
            Assert.AreEqual(dto1.Longitude, cinemaDto1.Longitude);
            Assert.AreEqual(dto2.Name, cinemaDto2.Name);
            Assert.AreEqual(dto2.Longitude, cinemaDto2.Longitude);
        }

        [TestMethod]
        public async Task GetAllCinemasWithoutMoviesTest()
        {
            //preparation
            string dbName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext1 = this.BuildContext(dbName);
            IMapper mapper = this.SeetingAutoMapper();

            Cinema entity1 = mapper.Map<Cinema>(
                new CinemaCreationUpdateDto()
                {
                    Name = "CC Unico Outlet - Pasto",
                    Latitude = 1.2055371680316427,
                    Longitude = -77.26046462525754
                });
            dbContext1.Cinemas.Add(entity1);

            Cinema entity2 = mapper.Map<Cinema>(
                new CinemaCreationUpdateDto()
                {
                    Name = "CC Unicentro - Pasto",
                    Latitude = 1.2169715540261588,
                    Longitude = -77.28860043409382
                });

            dbContext1.Cinemas.Add(entity2);
            Cinema entity3 = mapper.Map<Cinema>(
                new CinemaCreationUpdateDto()
                {
                    Name = "Cinemas Valle de Atriz",
                    Latitude = 1.2273724847208618,
                    Longitude = -77.28606410384705
                });
            dbContext1.Cinemas.Add(entity3);

            await dbContext1.SaveChangesAsync();

            int countEntities = await dbContext1.Cinemas.CountAsync();

            ICinemaServices cinemaServices = new CinemaServices(dbContext1, mapper, this.GeometryFactory);

            PaginationDto paginationDto = new()
            {
                Page = 1,
                RegistersPerPageQuantity = 50
            };

            //Moking a HttpContext:
            //Mock<HttpRequest> httpRequestMoq = new();
            //httpRequestMoq.Setup(x => x.Scheme).Returns("https");
            //httpRequestMoq.Setup(x => x.Host).Returns(HostString.FromUriComponent("https://localhost:8080"));
            //httpRequestMoq.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));

            //Mock<HttpResponse> httpResponseMoq = new(MockBehavior.Default);
            //Mock<IHeaderDictionary> headers = new();
            //httpResponseMoq.Setup(x => x.Headers).Returns(headers.Object);

            //Mock<HttpContext> httpContextMoq = new();
            //httpContextMoq.Setup(x => x.Request).Returns(httpRequestMoq.Object);
            //httpContextMoq.Setup(x => x.Response).Returns(httpResponseMoq.Object);

            //Providing a default HttpContext 
            HttpContext httpContext = new DefaultHttpContext();

            //Execution:
            ApplicationDbContext dbContext2 = this.BuildContext(dbName);
            IEnumerable<CinemaDto> cinemaDtos = await cinemaServices.GetAllAsync(httpContext, paginationDto);

            //Verification:
            Assert.AreEqual(countEntities, cinemaDtos.Count());
        }

        [TestMethod]
        public async Task GetCinemaByIdWithoutMoviesTest()
        {
            //Preparation:
            string databaseName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext1 = this.BuildContext(databaseName);
            IMapper mapper = this.SeetingAutoMapper();

            Cinema entity1 = mapper.Map<Cinema>(
                new CinemaCreationUpdateDto()
                {
                    Name = "CC Unico Outlet - Pasto",
                    Latitude = 1.2055371680316427,
                    Longitude = -77.26046462525754
                });

            Cinema entity2 = mapper.Map<Cinema>(
                new CinemaCreationUpdateDto()
                {
                    Name = "CC Unicentro - Pasto",
                    Latitude = 1.2169715540261588,
                    Longitude = -77.28860043409382
                });

            dbContext1.Cinemas.Add(entity1);
            dbContext1.Cinemas.Add(entity2);

            await dbContext1.SaveChangesAsync();

            int firstIndex = dbContext1.Cinemas.FirstOrDefault(x => x.Name == entity1.Name).Id;
            int secondIndex = dbContext1.Cinemas.FirstOrDefault(x => x.Name == entity2.Name).Id;

            ApplicationDbContext dbContext2 = this.BuildContext(databaseName);
            ICinemaServices cinemaServices = new CinemaServices(dbContext2, mapper, this.GeometryFactory);

            //Execution
            CinemaDetailsDto cinemaDto1 = await cinemaServices.GetByIdAsync(firstIndex);
            CinemaDetailsDto cinemaDto2 = await cinemaServices.GetByIdAsync(secondIndex);

            //Verification
            Assert.IsNotNull(cinemaDto1);
            Assert.IsNotNull(cinemaDto2);
            Assert.AreEqual(entity1.Name, cinemaDto1.Name);
            Assert.AreEqual(entity2.Name, cinemaDto2.Name);
        }

        [TestMethod]
        public async Task DeleteCinemaByIdTest()
        {
            //Preparation:
            string dbName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext1 = this.BuildContext(dbName);
            IMapper mapper = this.SeetingAutoMapper();

            Cinema entity1 = mapper.Map<Cinema>(
                new CinemaCreationUpdateDto()
                {
                    Name = "CC Unico Outlet - Pasto",
                    Latitude = 1.2055371680316427,
                    Longitude = -77.26046462525754d
                });

            Cinema entity2 = mapper.Map<Cinema>(
                new CinemaCreationUpdateDto()
                {
                    Name = "CC Unicentro - Pasto",
                    Latitude = 1.2169715540261588,
                    Longitude = -77.28860043409382
                });

            dbContext1.Cinemas.Add(entity1);
            dbContext1.Cinemas.Add(entity2);

            await dbContext1.SaveChangesAsync();

            int firstIndex = dbContext1.Cinemas.FirstOrDefault(x => x.Name == entity1.Name).Id;
            int secondIndex = dbContext1.Cinemas.FirstOrDefault(x => x.Name == entity2.Name).Id;

            ApplicationDbContext dbContext2 = this.BuildContext(dbName);

            ICinemaServices cinemaServices = new CinemaServices(dbContext2, mapper, this.GeometryFactory);

            //Execution:
            bool result1 = await cinemaServices.DeleteByIdAsync(firstIndex);
            bool result2 = await cinemaServices.DeleteByIdAsync(secondIndex);

            //Verification:
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.AreEqual(0, dbContext2.Cinemas.Count());
        }

        [TestMethod]
        public async Task UpdateCinemaWithoutMoviesTest()
        {
            //Preparation:
            string dbName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext1 = this.BuildContext(dbName);
            IMapper mapper = this.SeetingAutoMapper();

            Cinema entity1 = mapper.Map<Cinema>(
                new CinemaCreationUpdateDto()
                {
                    Name = "CC Unico Outlet - Pasto",
                    Latitude = 1.2055371680316427,
                    Longitude = -77.26046462525754d
                });

            Cinema entity2 = mapper.Map<Cinema>(
                new CinemaCreationUpdateDto()
                {
                    Name = "CC Unicentro - Pasto",
                    Latitude = 1.2169715540261588,
                    Longitude = -77.28860043409382
                });

            dbContext1.Cinemas.Add(entity1);
            dbContext1.Cinemas.Add(entity2);

            await dbContext1.SaveChangesAsync();

            int firstIndex = dbContext1.Cinemas.FirstOrDefault(x => x.Name == entity1.Name).Id;
            int secondIndex = dbContext1.Cinemas.FirstOrDefault(x => x.Name == entity2.Name).Id;

            ApplicationDbContext dbContext2 = this.BuildContext(dbName);

            ICinemaServices cinemaServices = new CinemaServices(dbContext2, mapper, this.GeometryFactory);
            string name1 = "CC Unico Outlet - Pasto dgsdgfs";
            string name2 = "CC Unicentro - Pasto gsdgsg";
            //Execution:

            bool result1 = await cinemaServices.UpdateAsync(firstIndex,
                new CinemaCreationUpdateDto()
                {
                    Name = name1,
                    Latitude = 1.2055371680316427,
                    Longitude = -77.26046462525754d
                });

            bool result2 = await cinemaServices.UpdateAsync(secondIndex,
                new CinemaCreationUpdateDto()
                {
                    Name = name2,
                    Latitude = 1.2169715540261588,
                    Longitude = -77.28860043409382
                });

            CinemaDetailsDto cinemaDto1 = await cinemaServices.GetByIdAsync(firstIndex);
            CinemaDetailsDto cinemaDto2 = await cinemaServices.GetByIdAsync(secondIndex);

            //Verification:
            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.AreEqual(name1, cinemaDto1.Name);
            Assert.AreEqual(name2, cinemaDto2.Name);
        }

        [TestMethod]
        public async Task GetNearbyTest()
        {
            //Preparation:          

            CinemaNearbyFilterDto nearbyFilterDto1 = new()
            {
                DistanceKm = 1,
                Latitude = 1.2031023515938222d,
                Longitude = -77.25549056647624d
            };

            CinemaNearbyFilterDto nearbyFilterDto2 = new()
            {
                DistanceKm = 5,
                Latitude = 1.2031023515938222d,
                Longitude = -77.25549056647624d
            };

            using (ApplicationDbContext context = LocalDbDatabaseInitializer.GetDbContextLocalDb(false))
            {
                //It is being used the data stored in the database.
                IMapper mapper = this.SeetingAutoMapper();
                ICinemaServices cinemaServices = new CinemaServices(context, mapper, this.GeometryFactory);

                //Execution:
                IEnumerable<CinemaNearbyDto> nearbyDtos1 = await cinemaServices.GetNearbyAsync(nearbyFilterDto1);
                IEnumerable<CinemaNearbyDto> nearbyDtos2 = await cinemaServices.GetNearbyAsync(nearbyFilterDto2);

                //Verification
                Assert.AreEqual(1, nearbyDtos1.Count());
                Assert.AreEqual(2, nearbyDtos2.Count());
            }
        }
    }
}
