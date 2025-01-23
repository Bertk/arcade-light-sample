using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Parsing;

namespace SampleConsoleApp
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            Option<int> delayOption = new("--delay") { Description = "delay in seconds", DefaultValueFactory = (_) => 42 };
            Option<string> messageOption = new("--message") { Required = true };

            RootCommand rootCommand = new("CommandLine example");
            rootCommand.Add(delayOption);
            rootCommand.Add(messageOption);
            rootCommand.Add(new HelpOption());
            rootCommand.Add(new VersionOption() { Description = "SampleConsoleApp version" });

            var parseResult = CommandLineParser.Parse(rootCommand, args);

            CommandLineConfiguration config = new CommandLineConfiguration(rootCommand);

            rootCommand.SetAction(_ =>
            {
                int delayOptionValue = parseResult.GetValue(delayOption);
                string messageOptionValue = parseResult.GetValue(messageOption) ?? "unknown";
                DoRootCommand(delayOptionValue, messageOptionValue);
            });

            await config.InvokeAsync(args).ConfigureAwait(false);

        }

        public static void DoRootCommand(int delay, string message)
        {
            Console.WriteLine($"--delay = {delay}");
            Console.WriteLine($"--message = {message}");
        }
    }
}
