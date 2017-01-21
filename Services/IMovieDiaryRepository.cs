using System.Collections.Generic;
using System.Threading.Tasks;
using MovieDiary.Models;

namespace MovieDiary.Services
{
    public interface IMovieDiaryRepository
    {
        IEnumerable<ApplicationUser> GetUsers();
        Task<ApplicationUser>GetUser(string id);
    }
}