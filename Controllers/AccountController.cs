using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieDiary.Data;
using MovieDiary.Models;
using MovieDiary.Models.AccountViewModels;
using MovieDiary.Services;

namespace MovieDiary.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMovieDiaryRepository _movieDiaryRepository;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IMovieDiaryRepository movieDiaryRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _movieDiaryRepository = movieDiaryRepository;
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false,
                    lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
//                    var adminRole = new IdentityRole("Admin");
//                    await _roleManager.CreateAsync(adminRole);
//                    await _userManager.AddToRoleAsync(user, "Admin");
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction(nameof(AdminController.Customers), "Admin");
                    }
                    return RedirectToAction(nameof(MoviesController.Seen), "Movies"); //
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName = model.Email, Email = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    await _signInManager.SignInAsync(user, isPersistent: false);

//                    ApplicationUser user = await _userManager.GetUserAsync(User);
                    return RedirectToAction(nameof(MoviesController.Seen), "Movies");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "home");
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
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
        public async  Task<IActionResult> Manage(ManageViewModel model)
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

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            ApplicationUser user = await _movieDiaryRepository.GetUser(id);
            if(user != null)
            {
                await _userManager.DeleteAsync(user);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin(string id)
        {
            ApplicationUser user = await _movieDiaryRepository.GetUser(id);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAdmin(string id)
        {
            ApplicationUser user = await _movieDiaryRepository.GetUser(id);
            if (user != null)
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
                return Ok();
            }
            return BadRequest();
        }





        #region Helpers

        private DbContextOptions<ApplicationDbContext> Options()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>();
            options.UseSqlite("Data Source=MovieDiary.db");
            return options.Options;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        #endregion
    }
}