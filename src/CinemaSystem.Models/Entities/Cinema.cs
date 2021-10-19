using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.Entities
{
    public class Cinema : IId
    {
        public int Id { get ; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public Point Location { get; set; }
        public IEnumerable<MoviesCinemas> MoviesCinemas { get; set; }
    }
}
