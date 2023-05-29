using System.CommandLine.Parsing;
using System.CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Serilog;
using Spectre.Console;

using SampleConsoleApp.Todo.Application.Common.Interfaces;

namespace SampleConsoleApp.Todo.Consoles
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers Serilog from configuration.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSerilog(this IServiceCollection services)
        {
            Log.Logger = CreateLogger(services);
            return services;

        }
        private static Serilog.Core.Logger CreateLogger(IServiceCollection services)
        {
            var scope = services.BuildServiceProvider();
            var parseResult = scope.GetRequiredService<ParseResult>();
            bool isSilentLogger = parseResult.HasOption(new Option<bool>("--silent"));
            var loggerConfiguration = new LoggerConfiguration()
                .ReadFrom.Configuration(scope.GetRequiredService<IConfiguration>());

            if (isSilentLogger)
            {
                _ = loggerConfiguration
                    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning);
            }

            return loggerConfiguration.CreateLogger();
        }
    }
}
