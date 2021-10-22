using AutoMapper;
using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Accounts;
using CinemaSystem.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.Services.AccountServices
{
    public class AccountServices : BaseServices, IAccountServices
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext dbContext;

        public AccountServices(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            ApplicationDbContext dbContext,
            IMapper mapper)
            : base(dbContext, mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.dbContext = dbContext;
        }

        public async Task<UserTokenDto> CreateUserAsync(UserInfoDto infoDto)
        {
            UserTokenDto tokenDto = null;
            IdentityUser user = new()
            {
                UserName = infoDto.Email,
                Email = infoDto.Email
            };

            IdentityResult result = await this.userManager.CreateAsync(user, infoDto.Password);
            if (result.Succeeded)
            {
                tokenDto = await this.CreateTokenAsync(infoDto);
            }

            return tokenDto;
        }

        public async Task<UserTokenDto> LoginAsync(UserInfoDto infoDto)
        {
            UserTokenDto userTokenDto = null;
            SignInResult result = await this.signInManager.PasswordSignInAsync(
                infoDto.Email,
                infoDto.Password,
                isPersistent: false,
                lockoutOnFailure: false);
            if (result.Succeeded)
            {
                userTokenDto = await this.CreateTokenAsync(infoDto);
            }

            return userTokenDto;
        }

        public async Task<UserTokenDto> RenovateTokenAsync(UserInfoDto infoDto)
        {
            UserTokenDto userTokenDto = await this.CreateTokenAsync(infoDto);

            return userTokenDto;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(HttpContext httpContext, PaginationDto paginationDto)
        {
            IEnumerable<UserDto> users = await this.GetAllAsync<IdentityUser, UserDto>(httpContext, paginationDto);

            return users;
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            IEnumerable<string> roles = await this.dbContext.Roles.Select(x => x.Name).ToListAsync();

            return roles;
        }

        public async Task<bool> AsignRoleAsync(RolEditDto dto)
        {
            IdentityUser user = await this.userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return false;
            }

            await this.userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, dto.RoleName));

            return true;
        }

        public async Task<bool> DeleteRoleAsync(RolEditDto dto)
        {
            IdentityUser user = await this.userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return false;
            }

            await this.userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, dto.RoleName));

            return true;
        }

        private async Task<UserTokenDto> CreateTokenAsync(UserInfoDto infoDto)
        {
            UserTokenDto tokenDto = null;

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, infoDto.Email),
                new Claim(ClaimTypes.Email, infoDto.Email)
            };

            IdentityUser identityUser = await this.userManager.FindByEmailAsync(infoDto.Email);
            if (identityUser == null)
            {
                identityUser = await this.userManager.FindByNameAsync(infoDto.Email);
                if (identityUser == null)
                {
                    return tokenDto;
                }
            }

            claims.Add(new Claim(ClaimTypes.NameIdentifier, identityUser.Id));
            IEnumerable<Claim> claimsDb = await this.userManager.GetClaimsAsync(identityUser);
            claims.AddRange(claimsDb);

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(this.configuration["jwt:key"]));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
            DateTime expiration = DateTime.UtcNow.AddYears(1);

            JwtSecurityToken jwt = new(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            tokenDto = new()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                Expiration = new DateTimeOffset(expiration)
            };

            return tokenDto;
        }
    }
}
