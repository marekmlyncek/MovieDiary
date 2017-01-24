using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieDiary.Models;
using MovieDiary.Models.MovieViewModels;
using MovieDiary.Names;
using MovieDiary.Services;

namespace MovieDiary.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IMovieDiaryRepository _movieDiaryRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public MoviesController(IMovieDiaryRepository movieDiaryRepository, UserManager<ApplicationUser> userManager)
        {
            _movieDiaryRepository = movieDiaryRepository;
            _userManager = userManager;
        }

        public IActionResult Index(string list)
        {
            var userId = _userManager.GetUserId(User);
            IEnumerable<Movie> movies = null;
            if (list == null)
            {
                movies = _movieDiaryRepository.GetUserMovies(userId);
            }
            else
            {
                movies = _movieDiaryRepository.GetUserMoviesInMovieList(userId,list);
            }
            var model = new IndexViewModel()
            {
                MovieLists = _movieDiaryRepository.GetUserMovieLists(userId),
                Movies = movies
            };
            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var userId = _userManager.GetUserId(User);
            var movie = _movieDiaryRepository.GetUserMovie(userId, id);
            if (movie == null)
            {
                movie = new Movie()
                {
                    Id = id,
                    ApplicationUserId = userId,
                    Rating = 0
                };
                _movieDiaryRepository.AddMovieToUser(movie);
            }
            var detail = new DetailViewModel
            {
                Movie = movie,
                Liked = _movieDiaryRepository.IsUserMovieInMovieList(userId, id,NameConvencion.ListLike),
                Seen = _movieDiaryRepository.IsUserMovieInMovieList(userId, id,NameConvencion.ListSeen),
                WatchList = _movieDiaryRepository.IsUserMovieInMovieList(userId, id,NameConvencion.ListWatchList)
            };
            var model = new IndexViewModel()
            {
                MovieLists = _movieDiaryRepository.GetUserMovieLists(userId),
                Movies = _movieDiaryRepository.GetUserMovies(userId),
                Detail = detail
        };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Detail(IndexViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            _movieDiaryRepository.UpdateRecord(userId,model.Detail);

            model.MovieLists = _movieDiaryRepository.GetUserMovieLists(userId);
            model.Movies = _movieDiaryRepository.GetUserMovies(userId);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var lists = _movieDiaryRepository.GetUserMovieListofMovie(userId, id);
            foreach (var item in lists)
            {
                _movieDiaryRepository.RemoveUserMovieFromMovieList(userId,id,item.Name);
            }
            _movieDiaryRepository.RemoveUserMovie(userId,id);
            return RedirectToAction("Index");
        }
    }
}