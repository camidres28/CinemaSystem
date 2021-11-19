using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Genres;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.GenreServices;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.UnitTests.ServicesTests
{
    [TestClass]
    public class GenreServicesTests : BaseServicesTests
    {
        [TestMethod]
        public async Task CreateAsyncTest()
        {
            //Preparation
            string dbName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext1 = this.BuildContext(dbName);
            IMapper mapper = this.SeetingAutoMapper();

            GenreCreateUpdateDto dto = new()
            {
                Name = "Horror"
            };

            IGenreServices genreServices = new GenreServices(mapper, dbContext1);

            //Execution
            GenreDto genreDto = await genreServices.CreateAsync(dto);

            //Verification
            Assert.AreEqual(dto.Name, genreDto.Name);            
        }

        [TestMethod]
        public async Task GetAllAsyncTest()
        {
            //Preparation

            string dbName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext1 = this.BuildContext(dbName);
            IMapper mapper = this.SeetingAutoMapper();

            dbContext1.Genres.Add(new() { Name = "Comedy" });
            dbContext1.Genres.Add(new() { Name = "Romantic" });
            dbContext1.Genres.Add(new() { Name = "Action" });
            dbContext1.Genres.Add(new() { Name = "Horror" });

            await dbContext1.SaveChangesAsync();

            ApplicationDbContext dbContext2 = this.BuildContext(dbName);
            IGenreServices genreServices = new GenreServices(mapper, dbContext2);

            PaginationDto paginationDto = new()
            {
                Page = 1,
                RegistersPerPageQuantity = 50
            };

            var httpRequestMoq = new Mock<HttpRequest>();
            httpRequestMoq.Setup(x => x.Scheme).Returns("https");
            httpRequestMoq.Setup(x => x.Host).Returns(HostString.FromUriComponent("https://localhost:8080"));
            httpRequestMoq.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/api"));

            var httpResponseMoq = new Mock<HttpResponse>(MockBehavior.Default);
            var headers = new Mock<IHeaderDictionary>();
            httpResponseMoq.Setup(x => x.Headers).Returns(headers.Object);

            var httpContextMoq = new Mock<HttpContext>();
            httpContextMoq.Setup(x => x.Request).Returns(httpRequestMoq.Object);
            httpContextMoq.Setup(x => x.Response).Returns(httpResponseMoq.Object);

            //Execution
            IEnumerable<GenreDto> genreDtos = await genreServices.GetAllAsync(httpContextMoq.Object, paginationDto);

            //Verification
            int count = dbContext1.Genres.Count();
            Assert.AreEqual(count, genreDtos.Count());
        }

        [TestMethod]
        public async Task GetByIdAsyncTest()
        {
            //Preparation
            string dbName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext1 = this.BuildContext(dbName);
            IMapper mapper = this.SeetingAutoMapper();

            string genre1 = "Comedy";
            string genre2 = "Romantic";

            dbContext1.Genres.Add(new() { Name = genre1 });
            dbContext1.Genres.Add(new() { Name = genre2 });

            await dbContext1.SaveChangesAsync();

            int firstIndex = dbContext1.Genres.FirstOrDefault(x => x.Name == genre1).Id;
            int secondIndex = dbContext1.Genres.FirstOrDefault(x => x.Name == genre2).Id;

            ApplicationDbContext dbContext2 = this.BuildContext(dbName);
            IGenreServices genreServices = new GenreServices(mapper, dbContext2);

            //Execution
            GenreDto genreDto1 = await genreServices.GetByIdAsync(firstIndex);
            GenreDto genreDto2 = await genreServices.GetByIdAsync(secondIndex);

            int non_existent = new Random().Next(secondIndex + 1, int.MaxValue);
            GenreDto genreDto3 = await genreServices.GetByIdAsync(non_existent);

            //Verification
            Assert.AreEqual(firstIndex, genreDto1.Id);
            Assert.AreEqual(secondIndex, genreDto2.Id);
            Assert.IsNull(genreDto3);
        }

        [TestMethod]
        public async Task DeleteByIdAsyncTest()
        {
            //Preparation
            string dbName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext1 = this.BuildContext(dbName);
            IMapper mapper = this.SeetingAutoMapper();

            string genre1 = "Comedy";

            dbContext1.Genres.Add(new() { Name = genre1 });

            await dbContext1.SaveChangesAsync();

            int firstIndex = dbContext1.Genres.FirstOrDefault(x => x.Name == genre1).Id;
            int non_existent = new Random().Next(firstIndex + 1, int.MaxValue);

            ApplicationDbContext dbContext2 = this.BuildContext(dbName);
            IGenreServices genreServices = new GenreServices(mapper, dbContext2);

            //Execution
            bool result1 = await genreServices.DeleteByIdAsync(firstIndex);
            bool result2 = await genreServices.DeleteByIdAsync(non_existent);

            //Verification
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        [TestMethod]
        public async Task UpdateByIdAsyncTest()
        {
            //Preparation
            string dbName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext1 = this.BuildContext(dbName);
            IMapper mapper = this.SeetingAutoMapper();

            string genre1 = "Comedy";

            dbContext1.Genres.Add(new() { Name = genre1 });

            await dbContext1.SaveChangesAsync();

            int firstIndex = dbContext1.Genres.FirstOrDefault(x => x.Name == genre1).Id;
            int non_existent = new Random().Next(firstIndex + 1, int.MaxValue);

            ApplicationDbContext dbContext2 = this.BuildContext(dbName);
            IGenreServices genreServices = new GenreServices(mapper, dbContext2);

            //Execution
            GenreDto genreDto = await genreServices.GetByIdAsync(firstIndex);
            genreDto.Name = "Dramma";
            bool result1 = await genreServices.UpdateAsync(firstIndex, genreDto);
            bool result2 = await genreServices.UpdateAsync(non_existent, genreDto);
            GenreDto genreDto1 = await genreServices.GetByIdAsync(firstIndex);

            //Verification
            Assert.IsTrue(result1);
            Assert.AreEqual("Dramma", genreDto1.Name);
            Assert.IsFalse(result2);
        }
    }
}
