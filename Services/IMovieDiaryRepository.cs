using System.Collections.Generic;
using System.Threading.Tasks;
using MovieDiary.Models;
using MovieDiary.Models.MovieViewModels;

namespace MovieDiary.Services
{
    public interface IMovieDiaryRepository
    {
        void CreteUserLists(string userId);
        IEnumerable<ApplicationUser> GetUsers();
        Task<ApplicationUser>GetUser(string id);
        IEnumerable<Movie> GetUserMovies(string userId);
        IEnumerable<MovieList> GetUserMovieLists(string userId);
        IEnumerable<MovieList> GetUserMovieListofMovie(string userId, int movieId);
        IEnumerable<Movie> GetUserMoviesInMovieList(string userId, string movieListName);
        Movie GetUserMovie(string userId, int movieId);
        bool IsUserMovieInMovieList(string userId, int movieId, string movieListName);
        void AddMovieToUser(Movie movie);
        void UpdateRecord(string userId, DetailViewModel model);
        void RemoveUserMovieFromMovieList(string userId, int movieId, string movieListName);
        void AddUserMovieToMovieList(string userId, int movieId, string movieListName);
        void RemoveUserMovie(string userId, int movieId);
    }
}