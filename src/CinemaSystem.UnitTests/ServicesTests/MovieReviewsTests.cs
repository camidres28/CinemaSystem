using AutoMapper;
using CinemaSystem.Models.DTOs.Reviews;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.MovieReviewsServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.UnitTests.ServicesTests
{
    [TestClass]
    public class MovieReviewsTests : BaseServicesTests
    {
        [TestMethod]
        public async Task UserCreatesMovieReviewSuccessfullTest()
        {
            //Preparation:
            string dbName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext = this.BuildContext(dbName);
            await this.CreateMovies(dbName);

            int movieId = await dbContext.Movies.Select(x => x.Id).SingleAsync();
            string comment = "It is the most wonderful movie I've ever been seen";

            ReviewCreateUpdateDto dto = new()
            {
                Score = 4,
                Comment = comment
            };

            ApplicationDbContext dbContext1 = this.BuildContext(dbName);
            IMapper mapper = this.SeetingAutoMapper();
            IMovieReviewsServices movieReviewsServices = new MovieReviewsServices(dbContext1, mapper);

            //Execution:
            ReviewReadingDto result = await movieReviewsServices.CreateAsync(movieId, this.BuildHttpContextWithClaims(), dto);

            //Veriication:
            Assert.IsNotNull(result);
            Assert.AreEqual(movieId, result.MovieId);
            Assert.AreEqual(comment, result.Comment);
            Assert.AreEqual(this.usuarioPorDefectoId, result.UserId);
        }

        [TestMethod]
        public async Task UserCreatesMovieReviewUnSuccessfullTest()
        {
            //Preparation:
            string dbName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext = this.BuildContext(dbName);
            await this.CreateMovies(dbName);

            int movieId = await dbContext.Movies.Select(x => x.Id).SingleAsync();
            string comment = "It is the most wonderful movie I've ever been seen";

            await dbContext.Reviews.AddAsync(new()
            {
                MovieId = movieId,
                UserId = this.usuarioPorDefectoId,
                Score = 4,
                Comment = comment
            });
            await dbContext.SaveChangesAsync();

            ReviewCreateUpdateDto dto = new()
            {
                Score = 5,
                Comment = comment + " ... And the scenes were amazing."
            };

            ApplicationDbContext dbContext1 = this.BuildContext(dbName);
            IMapper mapper = this.SeetingAutoMapper();
            IMovieReviewsServices movieReviewsServices = new MovieReviewsServices(dbContext1, mapper);

            //Execution:
            ReviewReadingDto result = await movieReviewsServices.CreateAsync(movieId, this.BuildHttpContextWithClaims(), dto);

            //Verification:
            Assert.IsNull(result);
        }

        private async Task CreateMovies(string dbName)
        {
            ApplicationDbContext dbContext = this.BuildContext(dbName);
            await dbContext.Movies.AddAsync(
                new()
                {
                    Title = "Spiderman no way home",
                    IsOnCinema = true,
                    ReleaseDate = new DateTimeOffset(new DateTime(2021, 12, 16))
                });
            await dbContext.SaveChangesAsync();
        }
    }
}
