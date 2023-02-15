using System.ComponentModel.DataAnnotations;

namespace TaskbySaurabhSirUI.Models
{
    public class RepoWithState
    {
        [Key]
        public int StateId { get; set; }
        public string? StateName { get; set; }
        public int? CountryId { get; set; }
    }
}
