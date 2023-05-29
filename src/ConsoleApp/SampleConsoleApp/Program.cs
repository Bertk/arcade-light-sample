using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine.Parsing;
using Microsoft.Extensions.Hosting;
using Serilog;

using SampleConsoleApp.Todo.Console.Commands;

var runner = BuildCommandLine()
    .UseHost(_ => Host.CreateDefaultBuilder(args), (builder) =>
    {
        builder.UseEnvironment("CLI")
        .UseSerilog()
        .ConfigureServices((hostContext, services) =>
        {
            var configuration = hostContext.Configuration;

        })
        .UseCommandHandler<CreateTodoListCommand, CreateTodoListCommand.Handler>()
        .UseCommandHandler<DeleteTodoListCommand, DeleteTodoListCommand.Handler>()
        .UseCommandHandler<ListTodosCommand, ListTodosCommand.Handler>()
        .UseCommandHandler<GetTodoListCommand, GetTodoListCommand.Handler>()
        .UseCommandHandler<SeedTodoItemsCommand, SeedTodoItemsCommand.Handler>();
    }).UseDefaults().Build();

return await runner.InvokeAsync(args);

    static CommandLineBuilder BuildCommandLine()
    {
        var root = new RootCommand();
        root.AddCommand(BuildTodoListCommands());
        root.AddCommand(BuildTodoItemsCommands());

        root.AddGlobalOption(new Option<bool>("--silent", "Disables diagnostics output"));
        root.Handler = CommandHandler.Create(() =>
        {
            root.Invoke("-h");
        });

        return new CommandLineBuilder(root);

        static Command BuildTodoListCommands()
        {
            var todolist = new Command("todolist", "Todo lists management")
        {
            new CreateTodoListCommand(),
            new DeleteTodoListCommand(),
            new GetTodoListCommand(),
            new ListTodosCommand(),
        };
            return todolist;
        }

        static Command BuildTodoItemsCommands()
        {
            var todoitem = new Command("todoitem", "Todo items management")
        {
            new SeedTodoItemsCommand()
        };
            return todoitem;
        }
    }


