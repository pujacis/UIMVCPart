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

        public virtual DbSet<City> RepoWithCities { get; set; }
        public virtual DbSet<Country> RepoWithCountries { get; set; }
        public virtual DbSet<State> RepoWithStates { get; set; }
    }
}
