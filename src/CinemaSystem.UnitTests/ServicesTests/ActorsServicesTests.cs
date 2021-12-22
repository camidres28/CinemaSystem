using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Actors;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.ActorServices;
using CinemaSystem.Services.StorageServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.UnitTests.ServicesTests
{
    [TestClass]
    public class ActorsServicesTests : BaseServicesTests
    {
        [TestMethod]
        public async Task CreateActorWithoutPhotoTest()
        {
            //Preparation
            string dbName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext1 = this.BuildContext(dbName);
            IMapper mapper = this.SeetingAutoMapper();

            Mock<IFileStorageServices> fileStorageMoq = new();
            fileStorageMoq.Setup(x => x.SaveFileAsync(null, null, null, null))
                .Returns(Task.FromResult("https://wwww"));

           
            string actorName = "Camilo Jojoa";
            DateTimeOffset actorBirthday = new DateTimeOffset(new DateTime(1990, 10, 28));
            ActorCreateUpdateDto actorDtoNew = new()
            {
                BirthDay = actorBirthday,
                Name = actorName
            };

            IActorServices actorServices = new ActorServices(mapper, dbContext1, fileStorageMoq.Object);

            //Execution:
            ActorDto actorDtoResult = await actorServices.CreateAsync(actorDtoNew);

            //Verification:
            ApplicationDbContext dbContext2 = this.BuildContext(dbName);
            Actor actor = await dbContext2.Actors.FirstOrDefaultAsync();            
            Assert.AreEqual(1, dbContext2.Actors.Count());
            Assert.AreEqual(actorName, actor.Name);
            Assert.AreEqual(actorBirthday, actor.BirthDay);
            Assert.AreEqual(0, fileStorageMoq.Invocations.Count);
            Assert.IsNull(actor.PhotoUrl);
        }

        [TestMethod]
        public async Task CreateActorWithPhoto()
        {
            //Preparation
            string dbName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext1 = this.BuildContext(dbName);
            IMapper mapper = this.SeetingAutoMapper();

            string actorName = "Camilo Jojoa";
            DateTimeOffset actorBirthday = new DateTimeOffset(new DateTime(1990, 10, 28));
            
            byte[] content = Encoding.UTF8.GetBytes("Test image");
            FormFile file = new(new MemoryStream(content), 0, content.Length, "photo", "photo.jpg");
            file.Headers = new HeaderDictionary();
            file.ContentType = "image/jpg";

            string fakeUrl = "https://www.imagetest.com";

            Mock<IFileStorageServices> fileStorageMoq = new();
            fileStorageMoq.Setup(x => x.SaveFileAsync(content, ".jpg", "actors", file.ContentType))
                .Returns(Task.FromResult(fakeUrl));

            IActorServices actorServices = new ActorServices(mapper, dbContext1, fileStorageMoq.Object);

            ActorCreateUpdateDto actorDtoNew = new()
            {
                BirthDay = actorBirthday,
                Name = actorName,
                Photo = file
            };
            //Execution:
            ActorDto actorDtoResult = await actorServices.CreateAsync(actorDtoNew);

            //Verification:
            ApplicationDbContext dbContext2 = this.BuildContext(dbName);
            Actor actor = await dbContext2.Actors.FirstOrDefaultAsync();
            Assert.AreEqual(1, dbContext2.Actors.Count());
            Assert.AreEqual(actorName, actor.Name);
            Assert.AreEqual(actorBirthday, actor.BirthDay);
            Assert.AreEqual(fakeUrl, actor.PhotoUrl);
            Assert.AreEqual(1, fileStorageMoq.Invocations.Count);            
        }

        [TestMethod]
        public async Task GetAllActorsTest()
        {
            //Preparation
            string dbName = Guid.NewGuid().ToString();
            ApplicationDbContext dbContext1 = this.BuildContext(dbName);
            IMapper mapper = this.SeetingAutoMapper();

            string actorName = "Camilo Jojoa";
            DateTimeOffset actorBirthday = new DateTimeOffset(new DateTime(1990, 10, 28));
            dbContext1.Actors.Add(new Actor()
            {
                Name = actorName,
                BirthDay = actorBirthday
            });

            await dbContext1.SaveChangesAsync();
            
            PaginationDto paginationDto = new()
            {
                Page = 1,
                RegistersPerPageQuantity = 10
            };

            ApplicationDbContext dbContext2 = this.BuildContext(dbName);
            IActorServices actorServices = new ActorServices(mapper, dbContext2, null);

            //Execution:
            IEnumerable<ActorDto> dtos = await actorServices.GetAllAsync(new DefaultHttpContext(), paginationDto);

            //Verification:
            Assert.AreEqual(1, dtos.Count());
        }
    }
}
