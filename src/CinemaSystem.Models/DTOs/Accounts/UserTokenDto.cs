using System;

namespace CinemaSystem.Models.DTOs.Accounts
{
    public class UserTokenDto
    {
        public string Token { get; set; }
        public DateTimeOffset Expiration { get; set; }
    }
}
