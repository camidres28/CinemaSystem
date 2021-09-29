using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.Models.DTOs.Actors
{
    public class ActorBaseDto
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        [Required]
        public DateTime BirthDay { get; set; }
    }
}
