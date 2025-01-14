using Microsoft.EntityFrameworkCore;
using SharedDataModels;

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

        public DbSet<Rental> Rentals { get; set; }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<Return> Returns { get; set; }

        public DbSet<Acceptation> Acceptations { get; set; }

        public DbSet<Description> Descriptions { get; set; }
    }
}
