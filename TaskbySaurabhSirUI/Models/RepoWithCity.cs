using System.ComponentModel.DataAnnotations;

namespace TaskbySaurabhSirUI.Models
{
    public class RepoWithCity
    {
        [Key]
        public int CityId { get; set; }
        public string? CityName { get; set; }
        public int? StateId { get; set; }
    }
}
