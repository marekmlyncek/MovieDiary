using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieDiary.Models;

namespace MovieDiary.Controllers
{
    public class HomeController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;
        public HomeController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Movies");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}