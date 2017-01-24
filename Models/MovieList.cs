using System.ComponentModel.DataAnnotations;

namespace MovieDiary.Models
{
    public class MovieList
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}