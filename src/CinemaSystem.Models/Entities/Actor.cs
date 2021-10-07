using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        [Required]
        public DateTimeOffset BirthDay { get; set; }
        public string PhotoUrl { get; set; }
    }
}
