namespace MovieDiary.Models
{
    public class MovieInMovieList
    {
        public int Id { get; set; }
        public Movie Movie { get; set; }
        public int MovieId { get; set; }
        public MovieList MovieList { get; set; }
        public int MovieListId { get; set; }
        public ApplicationUser User { get; set; }
        public string ApplicationUserId { get; set; }
    }
}