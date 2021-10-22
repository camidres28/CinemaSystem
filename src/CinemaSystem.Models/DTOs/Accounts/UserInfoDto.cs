using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.DTOs.Accounts
{
    public class UserInfoDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
