using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

namespace SampleConsoleApp
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            Option<int> delayOption = new Option<int>("--delay");
            delayOption.SetDefaultValue(42);

            Option<string> messageOption = new Option<string>("--message") { IsRequired = true };

            RootCommand rootCommand = new RootCommand("CommandLine example");
            rootCommand.Add(delayOption);
            rootCommand.Add(messageOption);

            rootCommand.SetHandler((delayOptionValue, messageOptionValue) =>
            {
                DoRootCommand(delayOptionValue, messageOptionValue);
            },
                delayOption, messageOption);

            CommandLineBuilder commandLineBuilder = new CommandLineBuilder(rootCommand);

            commandLineBuilder.UseDefaults();
            Parser parser = commandLineBuilder.Build();
            await parser.InvokeAsync(args).ConfigureAwait(false);
        }

        public static void DoRootCommand(int delay, string message)
        {
            Console.WriteLine($"--delay = {delay}");
            Console.WriteLine($"--message = {message}");
        }
    }
}
