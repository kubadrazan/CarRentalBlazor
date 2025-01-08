using Microsoft.EntityFrameworkCore;
using SharedDataModels;

namespace MiNICarRentalBrowser.Data
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<RentalBrowser> Rentals { get; set; }
        public DbSet<CarCache> CarsCache { get; set; }
    }
}

