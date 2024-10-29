using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RazorPagesTestSample.Data;

namespace RazorPagesTestSample.Tests.Utilities
{
    internal static class TestUtilities
    {
        #region snippet1
        public static DbContextOptions<AppDbContext> TestDbContextOptions()
        {
            // Create a new service provider to create a new in-memory database.
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance using an in-memory database and 
            // IServiceProvider that the context should resolve all of its 
            // services from.
            DbContextOptionsBuilder<AppDbContext> builder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("InMemoryDb")
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
        #endregion
    }
}
