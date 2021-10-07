using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.DTOs.Actors
{
    public class ActorBaseDto
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        [Required]
        public DateTimeOffset BirthDay { get; set; }
    }
}
