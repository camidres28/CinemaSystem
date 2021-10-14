using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CinemaSystem.Models.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MoviesGenres> MoviesGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoviesActors>()
                .HasKey(x => new { x.MovieId, x.ActorId });

            modelBuilder.Entity<MoviesGenres>()
                .HasKey(x => new { x.MovieId, x.GenreId });

            this.SeedDada(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedDada(ModelBuilder modelBuilder)
        {
            Genre adventure = new () { Id = 9, Name = "Adventure" };
            Genre animation = new () { Id = 10, Name = "Animation" };
            Genre suspence = new () { Id = 11, Name = "Suspence" };
            
            modelBuilder.Entity<Genre>()
                .HasData(new List<Genre>
                {
                    adventure, animation, suspence
                });

            Actor jimCarrey = new () { Id = 7, Name = "Jim Carrey", BirthDay = new DateTimeOffset(new DateTime( 1962, 01, 17)) };
            Actor chrisEvans = new () { Id = 8, Name = "Chris Evans", BirthDay = new DateTimeOffset(new DateTime(1981, 06, 13)) };
            Actor galGadoth = new() { Id = 9, Name = "Gal Gadot", BirthDay = new DateTimeOffset(new DateTime(1985, 04, 30)) };


            modelBuilder.Entity<Actor>()
                .HasData(new List<Actor>
                {
                    jimCarrey, galGadoth, chrisEvans
                });


            Movie iw = new ()
            {
                Id = 3,
                Title = "Avengers: Infinity Wars",
                IsOnCinema = true,
                ReleaseDate = new DateTimeOffset( new DateTime(2018, 04, 23))
            };

            Movie sonic = new ()
            {
                Id = 4,
                Title = "Sonic the Hedgehog",
                IsOnCinema = false,
                ReleaseDate = new DateTimeOffset(new DateTime(2020, 02, 28))
            };

            Movie emma = new ()
            {
                Id = 5,
                Title = "Emma",
                IsOnCinema = false,
                ReleaseDate = new DateTimeOffset(new DateTime(2020, 02, 21))
            };

            Movie wonderwoman = new ()
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
