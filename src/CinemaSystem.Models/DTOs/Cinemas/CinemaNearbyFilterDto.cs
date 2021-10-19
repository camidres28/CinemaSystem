using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaSystem.Models.DTOs.Cinemas
{
    public class CinemaNearbyFilterDto
    {
        private int distanceMaximumKm = 100000;
        private int distanceKm = 10;

        public int DistanceKm
        {
            get { return distanceKm; }
            set
            {
                if (value > this.distanceMaximumKm)
                {
                    value = this.distanceMaximumKm;
                }
                distanceKm = value; 
            }
        }

        [Range(-180, 180)]
        public double Longitude { get; set; }
        [Range(-90, 90)]
        public double Latitude { get; set; }
    }
}
