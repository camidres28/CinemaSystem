using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaSystem.Models.Entities
{
    public class MoviesCinemas
    {
        public int MovieId { get; set; }
        public int CinemaId { get; set; }
        public Movie Movie { get; set; }
        public Cinema Cinema { get; set; }
    }
}
