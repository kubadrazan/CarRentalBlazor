using Microsoft.EntityFrameworkCore;
using SharedDataModels;

namespace MiNICarRentalBrowser.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {
        }

        public DbSet<Localization> Localizations { get; set; }
        public DbSet<RentalBrowser> Rentals { get; set; }
    }
}

