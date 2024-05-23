using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.CommandLine.NamingConventionBinder;
using System.Collections.Generic;
using System.CommandLine.Help;
using System.IO;
using System.Linq;
using static System.Environment;

namespace SampleConsoleApp
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            CliOption<int> delayOption = new("--delay",  "delay in seconds");
            //delayOption.SetDefaultValue(42);

            CliOption<string> messageOption = new("--message") { Required = true } ;

            CliRootCommand rootCommand = new ("CommandLine example")
            {
                delayOption,
                messageOption
            };


            CliConfiguration config = new CliConfiguration(rootCommand);
            var result = config.Parse(args);

        }

        public static void DoRootCommand(int delay, string message)
        {
            Console.WriteLine($"--delay = {delay}");
            Console.WriteLine($"--message = {message}");
        }
    }
}
