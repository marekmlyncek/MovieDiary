using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieDiary.Data;
using MovieDiary.Models;

namespace MovieDiary.Services
{
    public class DatabaseMovieDiaryRepository :IMovieDiaryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public DatabaseMovieDiaryRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return _dbContext.Users.Include(x => x.Roles);
        }

        public Task<ApplicationUser> GetUser(string id)
        {
            return _dbContext.Users.FirstOrDefaultAsync(c => c.Id == id);
        }




    }
}