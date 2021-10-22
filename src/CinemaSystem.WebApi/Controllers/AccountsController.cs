using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Accounts;
using CinemaSystem.Services.AccountServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CinemaSystem.WebApi.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountServices accountServices;

        public AccountsController(IAccountServices accountServices)
        {
            this.accountServices = accountServices;
        }

        [HttpGet("users")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] PaginationDto paginationDto)
        {
            IEnumerable<UserDto> userDtos = await this.accountServices.GetAllUsersAsync(this.HttpContext, paginationDto);
            if (userDtos != null && userDtos.Any())
            {
                return Ok(userDtos);
            }

            return NoContent();
        }

        [HttpPost("users")]
        public async Task<ActionResult<UserTokenDto>> CreateUser([FromBody] UserInfoDto infoDto)
        {
            UserTokenDto userTokenDto = await this.accountServices.CreateUserAsync(infoDto);
            if (userTokenDto != null)
            {
                return userTokenDto;
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserTokenDto>> Login([FromBody] UserInfoDto infoDto)
        {
            UserTokenDto userTokenDto = await this.accountServices.LoginAsync(infoDto);
            if (userTokenDto != null)
            {
                return userTokenDto;
            }

            return BadRequest("Invalid login attempt");
        }

        [HttpPost("token")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserTokenDto>> RenovateToken()
        {
            UserInfoDto infoDto = new()
            {
                Email = this.HttpContext.User.Identity.Name
            };

            UserTokenDto userTokenDto = await this.accountServices.RenovateTokenAsync(infoDto);
            if (userTokenDto != null)
            {
                return userTokenDto;
            }

            return NoContent();
        }

        [HttpGet("roles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<string>>> GetRoles()
        {
            IEnumerable<string> roles = await this.accountServices.GetAllRolesAsync();

            if (roles != null && roles.Any())
            {
                return Ok(roles);
            }

            return NoContent();
        }

        [HttpPost("roles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> AsignRol([FromBody] RolEditDto dto)
        {
            bool result = await this.accountServices.AsignRoleAsync(dto);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("roles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult> DeleteRol([FromBody] RolEditDto dto)
        {
            bool result = await this.accountServices.DeleteRoleAsync(dto);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
