using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MovieDiary.Models.AdminViewModel;
using MovieDiary.Services;

namespace MovieDiary.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController :Controller
    {
        private readonly IMovieDiaryRepository _movieDiaryRepository;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(IMovieDiaryRepository movieDiaryRepository, RoleManager<IdentityRole> roleManager)
        {
            _movieDiaryRepository = movieDiaryRepository;
            _roleManager = roleManager;
        }


        [HttpGet]
        public IActionResult Customers()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var role = await _roleManager.FindByNameAsync("Admin");
            var models = new List<UserTableData>();
            var users = _movieDiaryRepository.GetUsers();
            foreach (var user in users)
            {
                var model = new UserTableData()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.FirstName + "&nbsp;" + user.LastName,
                    Phone = user.PhoneNumber
                };
                if (user.Roles.FirstOrDefault(c => c.RoleId == role.Id) != null)
                {
                    if (user.Email != "admin@moviediary.com")
                    {
                        model.Admin = 1;
                        model.Delete = 1;
                    }
                    else
                    {
                        model.Admin = 0;
                        model.Delete = 0;
                    }
                }
                else
                {
                    model.Admin = 2;
                    model.Delete = 1;
                }
                models.Add(model);

            }
            return Ok(models);
        }
    }
}