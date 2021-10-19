﻿// <auto-generated />
using System;
using CinemaSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CinemaSystem.Models.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211015153807_cinemas")]
    partial class cinemas
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CinemaSystem.Models.Entities.Actor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("BirthDay")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Actors");

                    b.HasData(
                        new
                        {
                            Id = 7,
                            BirthDay = new DateTimeOffset(new DateTime(1962, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -5, 0, 0, 0)),
                            Name = "Jim Carrey"
                        },
                        new
                        {
                            Id = 9,
                            BirthDay = new DateTimeOffset(new DateTime(1985, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -5, 0, 0, 0)),
                            Name = "Gal Gadot"
                        },
                        new
                        {
                            Id = 8,
                            BirthDay = new DateTimeOffset(new DateTime(1981, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -5, 0, 0, 0)),
                            Name = "Chris Evans"
                        });
                });

            modelBuilder.Entity("CinemaSystem.Models.Entities.Cinema", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Cinemas");
                });

            modelBuilder.Entity("CinemaSystem.Models.Entities.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = 9,
                            Name = "Adventure"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Animation"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Suspence"
                        });
                });

            modelBuilder.Entity("CinemaSystem.Models.Entities.Movie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsOnCinema")
                        .HasColumnType("bit");

                    b.Property<string>("PosterUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ReleaseDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.ToTable("Movies");

                    b.HasData(
                        new
                        {
                            Id = 3,
                            IsOnCinema = true,
                            ReleaseDate = new DateTimeOffset(new DateTime(2018, 4, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -5, 0, 0, 0)),
                            Title = "Avengers: Infinity Wars"
                        },
                        new
                        {
                            Id = 4,
                            IsOnCinema = false,
                            ReleaseDate = new DateTimeOffset(new DateTime(2020, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -5, 0, 0, 0)),
                            Title = "Sonic the Hedgehog"
                        },
                        new
                        {
                            Id = 5,
                            IsOnCinema = false,
                            ReleaseDate = new DateTimeOffset(new DateTime(2020, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -5, 0, 0, 0)),
                            Title = "Emma"
                        },
                        new
                        {
                            Id = 6,
                            IsOnCinema = true,
                            ReleaseDate = new DateTimeOffset(new DateTime(2020, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, -5, 0, 0, 0)),
                            Title = "Wonder Woman 1984"
                        });
                });

            modelBuilder.Entity("CinemaSystem.Models.Entities.MoviesActors", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("ActorId")
                        .HasColumnType("int");

                    b.Property<string>("Character")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("MovieId", "ActorId");

                    b.HasIndex("ActorId");

                    b.ToTable("MoviesActors");

                    b.HasData(
                        new
                        {
                            MovieId = 3,
                            ActorId = 8,
                            Character = "Steve Rogers",
                            Order = 2
                        },
                        new
                        {
                            MovieId = 4,
                            ActorId = 7,
                            Character = "Dr. Ivo Robotnik",
                            Order = 1
                        },
                        new
                        {
                            MovieId = 6,
                            ActorId = 9,
                            Character = "Dayana - Wonder Woman",
                            Order = 1
                        });
                });

            modelBuilder.Entity("CinemaSystem.Models.Entities.MoviesCinemas", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("CinemaId")
                        .HasColumnType("int");

                    b.HasKey("MovieId", "CinemaId");

                    b.HasIndex("CinemaId");

                    b.ToTable("MoviesCinemas");
                });

            modelBuilder.Entity("CinemaSystem.Models.Entities.MoviesGenres", b =>
                {
                    b.Property<int>("MovieId")
                        .HasColumnType("int");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.HasKey("MovieId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("MoviesGenres");

                    b.HasData(
                        new
                        {
                            MovieId = 3,
                            GenreId = 9
                        },
                        new
                        {
                            MovieId = 4,
                            GenreId = 9
                        },
                        new
                        {
                            MovieId = 5,
                            GenreId = 11
                        },
                        new
                        {
                            MovieId = 6,
                            GenreId = 11
                        },
                        new
                        {
                            MovieId = 6,
                            GenreId = 9
                        });
                });

            modelBuilder.Entity("CinemaSystem.Models.Entities.MoviesActors", b =>
                {
                    b.HasOne("CinemaSystem.Models.Entities.Actor", "Actor")
                        .WithMany("MoviesActors")
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CinemaSystem.Models.Entities.Movie", "Movie")
                        .WithMany("MoviesActors")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Actor");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("CinemaSystem.Models.Entities.MoviesCinemas", b =>
                {
                    b.HasOne("CinemaSystem.Models.Entities.Cinema", "Cinema")
                        .WithMany("MoviesCinemas")
                        .HasForeignKey("CinemaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CinemaSystem.Models.Entities.Movie", "Movie")
                        .WithMany("MoviesCinemas")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cinema");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("CinemaSystem.Models.Entities.MoviesGenres", b =>
                {
                    b.HasOne("CinemaSystem.Models.Entities.Genre", "Genre")
                        .WithMany("MoviesGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CinemaSystem.Models.Entities.Movie", "Movie")
                        .WithMany("MoviesGenres")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("CinemaSystem.Models.Entities.Actor", b =>
                {
                    b.Navigation("MoviesActors");
                });

            modelBuilder.Entity("CinemaSystem.Models.Entities.Cinema", b =>
                {
                    b.Navigation("MoviesCinemas");
                });

            modelBuilder.Entity("CinemaSystem.Models.Entities.Genre", b =>
                {
                    b.Navigation("MoviesGenres");
                });

            modelBuilder.Entity("CinemaSystem.Models.Entities.Movie", b =>
                {
                    b.Navigation("MoviesActors");

                    b.Navigation("MoviesCinemas");

                    b.Navigation("MoviesGenres");
                });
#pragma warning restore 612, 618
        }
    }
}
