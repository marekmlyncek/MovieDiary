using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieDiary.Services;

namespace MovieDiary.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IMovieDiaryRepository _movieDiaryRepository;

        public MoviesController(IMovieDiaryRepository movieDiaryRepository)
        {
            _movieDiaryRepository = movieDiaryRepository;
        }
        public IActionResult Seen()
        {
            //var model = _movieDiaryRepository.GetSeenMovies();

            return View();
        }

        public IActionResult Watch()
        {
            return View();
        }

        public IActionResult NewMovies(string search)
        {
            ViewData["search"] = search;

            return View();
        }

        public IActionResult Detail(string id)
        {
            return View();
        }

        public IActionResult AddToSeen(string id)
        {
            return View();
        }

        public IActionResult AddToWatchList(string id)
        {
            return View();
        }
    }
}