using System.ComponentModel.DataAnnotations;

namespace TaskbySaurabhSirUI.Models
{
    public class Login
    {
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}

