using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniCarRentalAPI.Data;
using MiNICarRentalBrowser.ApiKey;
using ThrowawayDb;

namespace DatabaseIntegrationTesting
{

    public class CustomWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Perform any configuration here that applies to all tests
            // such as replacing an external data store with a fake.
        }

        internal HttpClient CreateClientForDatabase(ThrowawayDatabase db)
            => GetFactoryForDatabase(db).CreateClient();


        internal IServiceProvider GetServiceProviderForDatabase(ThrowawayDatabase db)
            => GetFactoryForDatabase(db).Services;

        internal ThrowawayDatabase CreateThrowawayDb()
        {
            // Instance could be configured through environment variables.
            var db = ThrowawayDatabase.Create(SqlServerSettings.ConnectionString);
            //var db = ThrowawayDatabase.FromLocalInstance("(localdb)\\mssqllocaldb", "TEST_");

            var factory = GetFactoryForDatabase(db);

            // Apply migrations
            using var scope = factory.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<CarRentalContext>();
            context.Database.Migrate();

            return db;

        }

        private WebApplicationFactory<TProgram> GetFactoryForDatabase(ThrowawayDatabase db) =>
            WithWebHostBuilder(config =>
            {
                config.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<CarRentalContext>));

                    ArgumentNullException.ThrowIfNull(dbContextDescriptor);

                    services.Remove(dbContextDescriptor);

                    services.AddDbContext<CarRentalContext>(opt => opt.UseSqlServer(db.ConnectionString));
                });

            });
    }
}
