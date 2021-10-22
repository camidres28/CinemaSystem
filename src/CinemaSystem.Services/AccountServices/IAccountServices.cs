using CinemaSystem.Models.DTOs;
using CinemaSystem.Models.DTOs.Accounts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.Services.AccountServices
{
    public interface IAccountServices
    {
        Task<bool> AsignRoleAsync(RolEditDto dto);
        Task<bool> DeleteRoleAsync(RolEditDto dto);
        Task<IEnumerable<string>> GetAllRolesAsync();
        Task<UserTokenDto> LoginAsync(UserInfoDto infoDto);
        Task<UserTokenDto> CreateUserAsync(UserInfoDto infoDto);
        Task<UserTokenDto> RenovateTokenAsync(UserInfoDto infoDto);
        Task<IEnumerable<UserDto>> GetAllUsersAsync(HttpContext httpContext, PaginationDto paginationDto);
    }
}
