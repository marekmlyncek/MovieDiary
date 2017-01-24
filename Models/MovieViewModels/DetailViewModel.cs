namespace MovieDiary.Models.MovieViewModels
{
    public class DetailViewModel
    {
        public Movie Movie { get; set; }
        public bool Liked { get; set; }
        public bool Seen { get; set; }
        public bool WatchList { get; set; }

    }
}