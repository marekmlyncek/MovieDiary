using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieDiary.Models;
using MovieDiary.Models.AccountViewModels;

namespace MovieDiary.Controllers
{
    [Authorize]
    public class ManageController :Controller
    {
        private UserManager<ApplicationUser> _userManager;

        public ManageController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ManageViewModel model = new ManageViewModel();
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.AboutMe = user.AboutMe;
            model.PhoneNumber = user.PhoneNumber;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<IActionResult> Index(ManageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                user.LastName = model.LastName;
                user.FirstName = model.FirstName;
                user.AboutMe = model.AboutMe;
                user.PhoneNumber = model.PhoneNumber;
                await _userManager.UpdateAsync(user);

                return View(model);
            }
            return View(model);
        }
    }
}