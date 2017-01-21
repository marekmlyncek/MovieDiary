namespace MovieDiary.Models
{
    public class MovieInList
    {
        public int Id { get; set; }
        public Movie Movie { get; set; }
        public int MovieId { get; set; }
        public List List { get; set; }
        public int ListId { get; set; }
    }
}