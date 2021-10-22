namespace CinemaSystem.Models.DTOs.Reviews
{
    public class ReviewReadingDto : ReviewCreateUpdateDto
    {
        public int Id { get; set; }        
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
