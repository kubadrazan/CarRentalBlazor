using Microsoft.EntityFrameworkCore;
using MiniCarRentalAPI.Models;

namespace MiniCarRentalAPI.Data
{
    public class CarRentalContext : DbContext
    {
        public CarRentalContext(DbContextOptions<CarRentalContext> options) : base(options) 
        { 
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Localization> Localizations { get; set; }
        public DbSet<Rental> Rentals { get; set; }
    }
}
