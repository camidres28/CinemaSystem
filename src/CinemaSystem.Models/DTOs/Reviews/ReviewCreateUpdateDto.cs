using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.DTOs.Reviews
{
    public class ReviewCreateUpdateDto
    {
        [Required]
        public string Comment { get; set; }
        [Required]
        [Range(1, 5)]
        public int Score { get; set; }
    }
}
