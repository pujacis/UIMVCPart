using System.ComponentModel.DataAnnotations;

namespace TaskbySaurabhSirUI.Models
{
    public class Register
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
