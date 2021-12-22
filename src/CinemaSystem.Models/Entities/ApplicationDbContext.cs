using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

namespace CinemaSystem.Models.Entities
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<MoviesCinemas> MoviesCinemas { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoviesActors>()
                .HasKey(x => new { x.MovieId, x.ActorId });

            modelBuilder.Entity<MoviesGenres>()
                .HasKey(x => new { x.MovieId, x.GenreId });

            modelBuilder.Entity<MoviesCinemas>()
                .HasKey(x => new { x.MovieId, x.CinemaId });

            this.SeedDada(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedDada(ModelBuilder modelBuilder)
        {
            string rolAdminId = "de847a9b-d058-4771-8dac-3cac16764b8f";
            string userAdminId = "4b8670d0-7cfb-4e75-9824-9ec1aee20e6c";

            IdentityRole adminRole = new()
            {
                Id = rolAdminId,
                Name = "Admin",
                NormalizedName = "Admin"
            };

            PasswordHasher<IdentityUser> password = new();
            string userAdminEmail = "camilo.jojoa@mailinator.com";

            IdentityUser userAdmin = new()
            {
                Id = userAdminId,
                Email = userAdminEmail,
                UserName = userAdminEmail,
                NormalizedUserName = userAdminEmail,
                PasswordHash = password.HashPassword(null, "Admin123!")
            };

            //modelBuilder.Entity<IdentityUser>()
            //    .HasData(userAdmin);
            //modelBuilder.Entity<IdentityRole>()
            //    .HasData(adminRole);
            //modelBuilder.Entity<IdentityUserClaim<string>>()
            //    .HasData(new IdentityUserClaim<string>()
            //    {
            //        Id = 1,
            //        UserId = userAdminId,
            //        ClaimType = ClaimTypes.Role,
            //        ClaimValue = "Admin"
            //    });

            GeometryFactory geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            modelBuilder.Entity<Cinema>()
                .HasData(new List<Cinema>
                {
                    new(){Id = 2, Name = "CC Unico Outlet - Pasto", Location = geometryFactory.CreatePoint(new Coordinate(-77.25993641568172, 1.2051981387352946))},
                    new(){Id = 3, Name = "CC Unicentro - Pasto", Location = geometryFactory.CreatePoint(new Coordinate(-77.2885785290831, 1.2168629502902595))},
                    new(){Id = 4, Name = "CC Gran Plaza - Ipiales", Location = geometryFactory.CreatePoint(new Coordinate(-77.6489019621852, 0.830024343220887))},
                    new(){Id = 5, Name = "CC Ciudadela Unicentro - Cali", Location = geometryFactory.CreatePoint(new Coordinate(-76.53861779345984, 3.3744423246654374))},
                    new(){Id = 6, Name = "CC Premier Limonar - Cali", Location = geometryFactory.CreatePoint(new Coordinate(-76.54491406814019, 3.3950819652927047))},
                    new(){Id = 7, Name = "CC Chipichape - Cali", Location = geometryFactory.CreatePoint(new Coordinate(-76.52789708329091, 3.476271374637295))},
                    new(){Id = 8, Name = "CC Santafe - Bogota", Location = geometryFactory.CreatePoint(new Coordinate(-74.04591291868017, 4.766101549734984))}
                });

            Genre adventure = new() { Id = 9, Name = "Adventure" };
            Genre animation = new() { Id = 10, Name = "Animation" };
            Genre suspence = new() { Id = 11, Name = "Suspence" };

            modelBuilder.Entity<Genre>()
                .HasData(new List<Genre>
                {
                    adventure, animation, suspence
                });

            Actor jimCarrey = new() { Id = 7, Name = "Jim Carrey", BirthDay = new DateTimeOffset(new DateTime(1962, 01, 17)) };
            Actor chrisEvans = new() { Id = 8, Name = "Chris Evans", BirthDay = new DateTimeOffset(new DateTime(1981, 06, 13)) };
            Actor galGadoth = new() { Id = 9, Name = "Gal Gadot", BirthDay = new DateTimeOffset(new DateTime(1985, 04, 30)) };


            modelBuilder.Entity<Actor>()
                .HasData(new List<Actor>
                {
                    jimCarrey, galGadoth, chrisEvans
                });


            Movie iw = new()
            {
                Id = 3,
                Title = "Avengers: Infinity Wars",
                IsOnCinema = true,
                ReleaseDate = new DateTimeOffset(new DateTime(2018, 04, 23))
            };

            Movie sonic = new()
            {
                Id = 4,
                Title = "Sonic the Hedgehog",
                IsOnCinema = false,
                ReleaseDate = new DateTimeOffset(new DateTime(2020, 02, 28))
            };

            Movie emma = new()
            {
                Id = 5,
                Title = "Emma",
                IsOnCinema = false,
                ReleaseDate = new DateTimeOffset(new DateTime(2020, 02, 21))
            };

            Movie wonderwoman = new()
            {
                Id = 6,
                Title = "Wonder Woman 1984",
                IsOnCinema = true,
                ReleaseDate = new DateTimeOffset(new DateTime(2020, 08, 14))
            };

            modelBuilder.Entity<Movie>()
                .HasData(new List<Movie>
                {
                     iw, sonic, emma, wonderwoman
                });

            modelBuilder.Entity<MoviesGenres>().HasData(
                new List<MoviesGenres>()
                {
                    new MoviesGenres(){MovieId = iw.Id, GenreId = adventure.Id},
                    new MoviesGenres(){MovieId = sonic.Id, GenreId = adventure.Id},
                    new MoviesGenres(){MovieId = emma.Id, GenreId = suspence.Id},
                    new MoviesGenres(){MovieId = wonderwoman.Id, GenreId = suspence.Id},
                    new MoviesGenres(){MovieId = wonderwoman.Id, GenreId = adventure.Id},
                });

            modelBuilder.Entity<MoviesActors>().HasData(
                new List<MoviesActors>()
                {
                    new MoviesActors(){MovieId = iw.Id, ActorId = chrisEvans.Id, Character = "Steve Rogers", Order = 2},
                    new MoviesActors(){MovieId = sonic.Id, ActorId = jimCarrey.Id, Character = "Dr. Ivo Robotnik", Order = 1},
                    new MoviesActors(){MovieId = wonderwoman.Id, ActorId = galGadoth.Id, Character = "Dayana - Wonder Woman", Order = 1}
                });
        }
    }
}
