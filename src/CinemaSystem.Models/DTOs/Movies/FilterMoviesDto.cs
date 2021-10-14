namespace CinemaSystem.Models.DTOs.Movies
{
    public class FilterMoviesDto : PaginationDto
    {
        public string Title { get; set; }
        public int GenreId { get; set; }
        public bool OnCinema { get; set; }
        public bool FutureReleases { get; set; }
        public string OrderField { get; set; }
        public bool OrderAscending { get; set; } = true;
    }
}
