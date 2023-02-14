using System.ComponentModel.DataAnnotations;

namespace TaskbySaurabhSirUI.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        [Required(ErrorMessage = "User FirstName is required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string? LastName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "User LastName is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "User Country is required")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "User State is required")]
        public int StateId { get; set; }
        [Required(ErrorMessage = "User City is required")]
        public int CityId { get; set; }
        [Required(ErrorMessage = "User Address is required")]
        public string? Address { get; set; }
        //[Required(ErrorMessage = "User Name is required")]
         public string? FileName { get; set; }
        //public string FilePath { get; set; }

        // public string UploadFile { get; set; }
        [Required(ErrorMessage = "User Image is required")]
        public string? base64data { get; set; }
       // public string contentType { get; set; }
    }
}
