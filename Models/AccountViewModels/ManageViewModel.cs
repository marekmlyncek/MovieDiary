using System.ComponentModel.DataAnnotations;

namespace MovieDiary.Models.AccountViewModels
{
    public class ManageViewModel
    {
        public string PhoneNumber { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Display(Name = "About Me")]
        [MaxLength(5000)]
        public string AboutMe { get; set; }
    }
}