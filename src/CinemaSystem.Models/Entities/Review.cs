using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.Entities
{
    public class Review : IId
    {
        public int Id { get ; set ; }
        [Required]
        public string Comment { get; set; }
        [Required]
        [Range(1, 5)]
        public int Score { get; set; }
        [Required]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        [Required]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

    }
}
