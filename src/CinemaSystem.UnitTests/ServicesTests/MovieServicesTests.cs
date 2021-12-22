using AutoMapper;
using CinemaSystem.Models.DTOs.Movies;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.MovieServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.UnitTests.ServicesTests
{
    [TestClass]
    public class MovieServicesTests : BaseServicesTests
    {
        private async Task<string> CreateTestData()
        {
            string databaseName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext = this.BuildContext(databaseName);

            List<Movie> moviesEntities = new()
            {
                new() { Title = "Avengers 1", ReleaseDate = new(new DateTime(2012, 05, 12)), IsOnCinema = true },
                new() { Title = "Avengers 2", ReleaseDate = new(new DateTime(2013, 05, 12)), IsOnCinema = true },
                new() { Title = "Avengers 3", ReleaseDate = new(new DateTime(2014, 05, 12)), IsOnCinema = true },
                new() { Title = "Avengers 4", ReleaseDate = new(DateTime.Today.AddMonths(2)), IsOnCinema = false },
                new() { Title = "Avengers 5", ReleaseDate = new(DateTime.Today.AddMonths(6)), IsOnCinema = false }
            };

            List<Genre> genresEntities = new()
            {
                new() { Name = "Action" },
                new() { Name = "Drama" },
                new() { Name = "Comedy" },
                new() { Name = "Horror" },
            };

            await dbContext.Genres.AddRangeAsync(genresEntities);
            await dbContext.Movies.AddRangeAsync(moviesEntities);
            await dbContext.SaveChangesAsync();

            int actionGenre_id = dbContext.Genres.SingleOrDefaultAsync(x => x.Name == "Action").Id;
            List<int> movies_ids = await dbContext.Movies.Select(x => x.Id).ToListAsync();

            List<MoviesGenres> moviesGenres = new();

            movies_ids.ForEach(x => moviesGenres.Add(new() { GenreId = actionGenre_id, MovieId = x }));
            await dbContext.MoviesGenres.AddRangeAsync(moviesGenres);
            await dbContext.SaveChangesAsync();

            return databaseName;
        }

        [TestMethod]
        public async Task GetByFiltering()
        {
            //Preparation
            string dbName = await this.CreateTestData();
            IMapper mapper = this.SeetingAutoMapper();
            ApplicationDbContext dbContext = BuildContext(dbName);

            FilterMoviesDto filterDto1 = new()
            {
                Title = "Avengers",
            };

            FilterMoviesDto filterDto2 = new()
            {
                Title = "Avengers 1",
            };

            FilterMoviesDto filterDto3 = new()
            {
                Title = "Avengers",
                OnCinema = true
            };

            FilterMoviesDto filterDto4 = new()
            {
                Title = "Avengers", 
                OnCinema = false
            };

            int actionGenreId = dbContext.Genres.SingleOrDefault(x => x.Name == "Action").Id;
            int horrorGenreId = dbContext.Genres.SingleOrDefault(x => x.Name == "Horror").Id;

            FilterMoviesDto filterDto5 = new()
            {
                GenreId = actionGenreId
            };

            FilterMoviesDto filterDto6 = new()
            {
                GenreId = horrorGenreId
            };

            FilterMoviesDto filterDto7 = new()
            {
                OrderField = "Unknown"
            };

            Mock<ILogger<MovieServices>> loggerMoq = new();
            IMovieServices movieServices = new MovieServices(mapper, dbContext, null, loggerMoq.Object);

            //Execution
            IEnumerable<MovieDto> movieDtos1 = await movieServices.GetByFilteringAsync(new DefaultHttpContext(), filterDto1);
            IEnumerable<MovieDto> movieDtos2 = await movieServices.GetByFilteringAsync(new DefaultHttpContext(), filterDto2);
            IEnumerable<MovieDto> movieDtos3 = await movieServices.GetByFilteringAsync(new DefaultHttpContext(), filterDto3);
            IEnumerable<MovieDto> movieDtos4 = await movieServices.GetByFilteringAsync(new DefaultHttpContext(), filterDto4);
            IEnumerable<MovieDto> movieDtos5 = await movieServices.GetByFilteringAsync(new DefaultHttpContext(), filterDto5);
            IEnumerable<MovieDto> movieDtos6 = await movieServices.GetByFilteringAsync(new DefaultHttpContext(), filterDto6);
            IEnumerable<MovieDto> movieDtos7 = await movieServices.GetByFilteringAsync(new DefaultHttpContext(), filterDto7);

            //Verification:
            Assert.AreEqual(5, movieDtos1.Count());
            Assert.AreEqual(1, movieDtos2.Count());
            Assert.AreEqual(3, movieDtos3.Count());
            Assert.AreEqual(2, movieDtos4.Count());
            Assert.AreEqual(5, movieDtos5.Count());
            Assert.AreEqual(0, movieDtos6.Count());
            Assert.AreEqual(5, movieDtos7.Count());
            Assert.AreEqual(1, loggerMoq.Invocations.Count);
        }
    }
}
