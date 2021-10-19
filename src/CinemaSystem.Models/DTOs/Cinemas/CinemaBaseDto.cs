using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.DTOs.Cinemas
{
    public class CinemaBaseDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }
        [Range(-90, 90)]
        public double Latitude { get; set; }
    }
}
