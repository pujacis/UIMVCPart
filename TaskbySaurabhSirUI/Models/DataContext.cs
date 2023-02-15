using Microsoft.EntityFrameworkCore;
using TaskbySaurabhSirUI.Models;

namespace TaskbySaurabhSirUI
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public  DbSet<RepoWithCity> RepoWithCities { get; set; }
        public  DbSet<RepoWithCountry> RepoWithCountries { get; set; }
        public  DbSet<RepoWithState> RepoWithStates { get; set; }
    }
}
