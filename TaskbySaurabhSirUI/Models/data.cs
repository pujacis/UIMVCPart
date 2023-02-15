using Microsoft.EntityFrameworkCore;
using TaskbySaurabhSirUI.Models;

namespace TaskbySaurabhSirUI
{
    public class data : DbContext
    {
        public data(DbContextOptions<data> options)
            : base(options)
        {
        }

        public virtual DbSet<RepoWithCity> RepoWithCities { get; set; }
        public virtual DbSet<RepoWithCountry> RepoWithCountries { get; set; }
        public virtual DbSet<RepoWithState> RepoWithStates { get; set; }
    }
}
