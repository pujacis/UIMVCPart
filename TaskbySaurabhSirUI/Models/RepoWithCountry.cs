using System.ComponentModel.DataAnnotations;

namespace TaskbySaurabhSirUI.Models
{
    public class RepoWithCountry
    {
        [Key]
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
    }
}
