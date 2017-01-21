namespace MovieDiary.Models
{
    public class Movie
    {
        public string Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}