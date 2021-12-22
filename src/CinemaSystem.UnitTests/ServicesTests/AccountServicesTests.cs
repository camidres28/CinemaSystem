using AutoMapper;
using CinemaSystem.Models.DTOs.Accounts;
using CinemaSystem.Models.Entities;
using CinemaSystem.Services.AccountServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaSystem.UnitTests.ServicesTests
{
    [TestClass]
    public class AccountServicesTests : BaseServicesTests
    {
        [TestMethod]
        public async Task CreateUserTest()
        {
            //Preparation:
            string dbName = Guid.NewGuid().ToString();

            //Execution:
            await this.CreateUserHelper(dbName);

            //Verification:
            ApplicationDbContext dbContext = this.BuildContext(dbName);
            int countUsers = dbContext.Users.Count();
            string email = dbContext.Users.SingleOrDefault().Email;
            Assert.AreEqual(1, countUsers);
            Assert.AreEqual("camilo.jojoa@email.com", email);
        }

        [TestMethod]
        public async Task UserLogOnUnsuccessful()
        {
            //Preparation:
            string dbName = Guid.NewGuid().ToString();
            await this.CreateUserHelper(dbName);

            UserInfoDto dto = new()
            {
                Email = "camilo.jojoa@email.com",
                Password = "Admin1234!"
            };

            IAccountServices accountServices = this.BuidAccountServices(dbName);

            //Execution:
            UserTokenDto userTokenDto = await accountServices.LoginAsync(dto);

            //Verification:
            Assert.IsNull(userTokenDto);
        }

        [TestMethod]
        public async Task UserLogOnSuccessful()
        {
            //Preparation:
            string dbName = Guid.NewGuid().ToString();
            await this.CreateUserHelper(dbName);

            UserInfoDto dto = new()
            {
                Email = "camilo.jojoa@email.com",
                Password = "Admin123!"
            };

            IAccountServices accountServices = this.BuidAccountServices(dbName);

            //Execution:
            UserTokenDto userTokenDto = await accountServices.LoginAsync(dto);

            //Verification:
            Assert.IsNotNull(userTokenDto);
            Assert.IsNotNull(userTokenDto.Token);
        }

        private async Task CreateUserHelper(string dbName)
        {
            IAccountServices accountServices = this.BuidAccountServices(dbName);
            UserInfoDto userInfoDto = new()
            {
                Email = "camilo.jojoa@email.com",
                Password = "Admin123!"
            };

            await accountServices.CreateUserAsync(userInfoDto);
        }

        private IAccountServices BuidAccountServices(string dbName)
        {
            ApplicationDbContext dbContext = this.BuildContext(dbName);
            UserStore<IdentityUser> myUserStore = new(dbContext);
            UserManager<IdentityUser> userManager = this.BuildUserManager(myUserStore);
            IMapper mapper = this.SeetingAutoMapper();

            HttpContext httpContext = new DefaultHttpContext();
            this.MockAuth(httpContext);
            SignInManager<IdentityUser> signInManager = this.SetupSignInManager(userManager, httpContext);
            Dictionary<string, string> myConfig = new()
            {
                {
                    "jwt:key",
                    "QURBVSPIRJ2643JNXA753VMDPW02KASHAHDA63BMCLKSJFQ5LNGSMNVXGY5POQOIR349VZX32KFHDGT53SDFGA"
                }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfig)
                .Build();

            AccountServices accountServices = new(userManager, signInManager, configuration, dbContext, mapper);

            return accountServices;
        }


        // Source: https://github.com/dotnet/aspnetcore/blob/master/src/Identity/test/Shared/MockHelpers.cs
        // Source: https://github.com/dotnet/aspnetcore/blob/master/src/Identity/test/Identity.Test/SignInManagerTest.cs
        // Some code was modified to be adapted to our project.

        private UserManager<TUser> BuildUserManager<TUser>(IUserStore<TUser> store = null) where TUser : class
        {
            store = store ?? new Mock<IUserStore<TUser>>().Object;
            Mock<IOptions<IdentityOptions>> options = new();
            IdentityOptions idOptions = new();
            idOptions.Lockout.AllowedForNewUsers = false;

            options.Setup(o => o.Value).Returns(idOptions);

            List<IUserValidator<TUser>> userValidators = new();

            Mock<IUserValidator<TUser>> validator = new();
            userValidators.Add(validator.Object);
            List<PasswordValidator<TUser>> pwdValidators = new();
            pwdValidators.Add(new PasswordValidator<TUser>());

            UserManager<TUser> userManager = new(store, options.Object, new PasswordHasher<TUser>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<TUser>>>().Object);

            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<TUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();

            return userManager;
        }

        private SignInManager<TUser> SetupSignInManager<TUser>(UserManager<TUser> manager,
            HttpContext context, ILogger logger = null, IdentityOptions identityOptions = null,
            IAuthenticationSchemeProvider schemeProvider = null) where TUser : class
        {
            Mock<IHttpContextAccessor> contextAccessor = new();
            contextAccessor.Setup(a => a.HttpContext).Returns(context);
            identityOptions = identityOptions ?? new IdentityOptions();
            Mock<IOptions<IdentityOptions>> options = new();
            options.Setup(a => a.Value).Returns(identityOptions);
            UserClaimsPrincipalFactory<TUser> claimsFactory = new(manager, options.Object);
            schemeProvider = schemeProvider ?? new Mock<IAuthenticationSchemeProvider>().Object;
            SignInManager<TUser> sm = new(manager, contextAccessor.Object, claimsFactory, options.Object, null, schemeProvider, new DefaultUserConfirmation<TUser>());
            sm.Logger = logger ?? (new Mock<ILogger<SignInManager<TUser>>>()).Object;
            return sm;
        }

        private Mock<IAuthenticationService> MockAuth(HttpContext context)
        {
            Mock<IAuthenticationService> auth = new();
            context.RequestServices = new ServiceCollection().AddSingleton(auth.Object).BuildServiceProvider();
            return auth;
        }
    }
}
