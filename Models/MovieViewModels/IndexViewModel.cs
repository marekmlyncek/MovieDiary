using System.Collections.Generic;

namespace MovieDiary.Models.MovieViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<MovieList> MovieLists { get; set; }
        public DetailViewModel Detail { get; set; }
    }
}