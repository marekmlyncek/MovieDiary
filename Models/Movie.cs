namespace MovieDiary.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Poster { get; set; }
        public string Memo { get; set; }
        public int Rating { get; set; }
    }
}