using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieDiary.Data;
using MovieDiary.Models;
using MovieDiary.Models.MovieViewModels;
using MovieDiary.Names;

namespace MovieDiary.Services
{
    public class DatabaseMovieDiaryRepository :IMovieDiaryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseMovieDiaryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreteUserLists(string userId)
        {
            _dbContext.MovieLists.Add(new MovieList()
            {
                ApplicationUserId = userId,
                Name = NameConvencion.ListSeen
            });
            _dbContext.MovieLists.Add(new MovieList()
            {
                ApplicationUserId = userId,
                Name = NameConvencion.ListLike
            });
            _dbContext.MovieLists.Add(new MovieList()
            {
                ApplicationUserId = userId,
                Name = NameConvencion.ListWatchList
            });
            _dbContext.SaveChanges();
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return _dbContext.Users.Include(x => x.Roles);
        }

        public Task<ApplicationUser> GetUser(string id)
        {
            return _dbContext.Users.FirstOrDefaultAsync(c => c.Id == id);
        }

        public IEnumerable<Movie> GetUserMovies(string userId)
        {
            return _dbContext.Movies.Where(c => c.ApplicationUserId == userId);
        }

        public IEnumerable<MovieList> GetUserMovieLists(string userId)
        {
            return _dbContext.MovieLists.Where(c => c.ApplicationUserId == userId);
        }

        public IEnumerable<MovieList> GetUserMovieListofMovie(string userId, int movieId)
        {
            return _dbContext.MoviesInMovieLists.Include(c => c.MovieList)?.Where(c => c.ApplicationUserId == userId)
                ?.Where(c => c.MovieId == movieId)
                ?.Select(c => c.MovieList);
        }

        public IEnumerable<Movie> GetUserMoviesInMovieList(string userId, string movieListName)
        {
            var movieListId = _dbContext.MovieLists?.Where(c => c.ApplicationUserId == userId)
                ?.FirstOrDefault(c => c.Name == movieListName);
            if (movieListId == null)
            {
                return null;
            }
            return _dbContext.MoviesInMovieLists.Include(c => c.Movie)
                ?.Where(c => c.ApplicationUserId == userId)
                ?.Where(c => c.MovieListId == movieListId.Id)
                ?.Select(c => c.Movie);
        }

        public Movie GetUserMovie(string userId, int movieId)
        {
            return _dbContext.Movies?.Where(c => c.ApplicationUserId == userId)?.FirstOrDefault(c => c.Id == movieId);
        }

        public bool IsUserMovieInMovieList(string userId, int movieId, string movieListName)
        {
            var movieListId = _dbContext.MovieLists.Where(c => c.ApplicationUserId == userId)
                ?.FirstOrDefault(c => c.Name == movieListName);
            if (movieListId == null)
            {
                return false;
            }
            var movieInList = _dbContext.MoviesInMovieLists.Where(c => c.ApplicationUserId == userId)
                ?.Where(c => c.MovieListId == movieListId.Id)
                ?.FirstOrDefault(c => c.MovieId == movieId);
            if (movieInList == null)
            {
                return false;
            }
            return true;
        }

        public void AddMovieToUser(Movie movie)
        {
            _dbContext.Movies.Add(movie);
            _dbContext.SaveChanges();
        }

        public void UpdateRecord(string userId, DetailViewModel model)
        {
            var movie = _dbContext.Movies.FirstOrDefault(c => c.ApplicationUserId == userId && c.Id == model.Movie.Id);
            movie.Memo = model.Movie.Memo;
            movie.Rating = model.Movie.Rating;
            _dbContext.Movies.Update(movie);
            _dbContext.SaveChanges();

            if (IsUserMovieInMovieList(userId, movie.Id, NameConvencion.ListLike) != model.Liked)
            {
                if (model.Liked)
                {
                    AddUserMovieToMovieList(userId, movie.Id, NameConvencion.ListLike);
                }
                else
                {
                    RemoveUserMovieFromMovieList(userId,movie.Id,NameConvencion.ListLike);
                }
            }
            if (IsUserMovieInMovieList(userId, movie.Id, NameConvencion.ListSeen) != model.Seen)
            {
                if (model.Seen)
                {
                    AddUserMovieToMovieList(userId, movie.Id, NameConvencion.ListSeen);
                }
                else
                {
                    RemoveUserMovieFromMovieList(userId,movie.Id,NameConvencion.ListSeen);
                }
            }
            if (IsUserMovieInMovieList(userId, movie.Id, NameConvencion.ListWatchList) != model.WatchList)
            {
                if (model.WatchList)
                {
                    AddUserMovieToMovieList(userId, movie.Id, NameConvencion.ListWatchList);
                }
                else
                {
                    RemoveUserMovieFromMovieList(userId,movie.Id,NameConvencion.ListWatchList);
                }
            }
        }

        public void RemoveUserMovieFromMovieList(string userId, int movieId, string movieListName)
        {
            var movieListId = _dbContext.MovieLists.Where(c => c.ApplicationUserId == userId)
                .FirstOrDefault(c => c.Name == movieListName);
            if (movieListId != null)
            {
                var movieinlist = _dbContext.MoviesInMovieLists.Where(c => c.ApplicationUserId == userId)
                    .FirstOrDefault(c => c.MovieId == movieId && c.MovieListId == movieListId.Id);
                _dbContext.MoviesInMovieLists.Remove(movieinlist);
                _dbContext.SaveChanges();
            }
        }

        public void AddUserMovieToMovieList(string userId, int movieId, string movieListName)
        {
            var movieListId = _dbContext.MovieLists.Where(c => c.ApplicationUserId == userId)
                .FirstOrDefault(c => c.Name == movieListName);

            _dbContext.MoviesInMovieLists.Add(new MovieInMovieList()
            {
                ApplicationUserId = userId,
                MovieId = movieId,
                MovieList = movieListId
            });
            _dbContext.SaveChanges();
        }

        public void RemoveUserMovie(string userId, int movieId)
        {
            var movie =_dbContext.Movies.Where(c => c.ApplicationUserId == userId).FirstOrDefault(c => c.Id == movieId);
            if (movie != null)
            {
                _dbContext.Movies.Remove(movie);
                _dbContext.SaveChanges();
            }
        }
    }
}