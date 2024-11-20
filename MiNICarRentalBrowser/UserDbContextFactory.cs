using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using MiNICarRentalBrowser.Data;

namespace MiNICarRentalBrowser
{
    public class UserDbContextFactory : IDesignTimeDbContextFactory<UsersContext>
    {
        public UsersContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UsersContext>();

            // Get the connection string from appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            return new UsersContext(optionsBuilder.Options);
        }
    }
}
