using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TestTask.Infrastructure.Repositories
{
    public class TestTaskDbContextFactory : IDesignTimeDbContextFactory<TestTaskDbContext>
    {
        public TestTaskDbContext CreateDbContext(string[] args)
        {
            // Build configuration to read from appsettings.json.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TestTaskDbContext>();

            // Configure the DbContext to use PostgreSQL, using the connection string from configuration.
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            // Return a new instance of TaskDBContext with the configured options.
            return new TestTaskDbContext(optionsBuilder.Options);
        }
    }
}
