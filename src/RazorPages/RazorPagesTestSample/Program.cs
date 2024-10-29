using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RazorPagesTestSample.Data;

namespace RazorPagesTestSample
{
#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable
#pragma warning disable S1118
    public class Program
#pragma warning restore S1118
#pragma warning restore CA1052 // Static holder types should be Static or NotInheritable
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                AppDbContext db = services.GetRequiredService<AppDbContext>();

                _ = db.Database.EnsureCreated();

                if (!db.Messages.Any())
                {
#pragma warning disable CA1031 // Do not catch general exception types
                    try
                    {
                        db.Initialize();
                    }
                    catch (Exception ex)
                    {
                        ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                        logger.LogError(ex, "An error occurred seeding the database. Error: {Message}", ex.Message);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
                    }
#pragma warning restore CA1031 // Do not catch general exception types
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    _ = webBuilder.UseStartup<Startup>();
                });
    }
}
