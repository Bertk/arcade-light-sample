using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

class Program
{
    static async Task Main(string[] args)
    {
        var delayOption = new Option<int>("--delay");
        delayOption.SetDefaultValue(42);

        var messageOption = new Option<string>("--message") { IsRequired=true};

        var rootCommand = new RootCommand("CommandLine example");
        rootCommand.Add(delayOption);
        rootCommand.Add(messageOption);

        rootCommand.SetHandler((delayOptionValue, messageOptionValue) =>
        {
            DoRootCommand(delayOptionValue, messageOptionValue);
        },
            delayOption, messageOption);

        var commandLineBuilder = new CommandLineBuilder(rootCommand);

        commandLineBuilder.UseDefaults();
        var parser = commandLineBuilder.Build();
        await parser.InvokeAsync(args).ConfigureAwait(false);
    }

    public static void DoRootCommand(int delay, string message)
    {
        Console.WriteLine($"--delay = {delay}");
        Console.WriteLine($"--message = {message}");
    }
}
