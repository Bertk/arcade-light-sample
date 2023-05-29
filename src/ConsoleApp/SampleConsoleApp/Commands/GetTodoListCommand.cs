using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using SampleConsoleApp.Todo.Application.TodoLists.Queries.GetTodo;
using MediatR;
using Spectre.Console;

namespace SampleConsoleApp.Todo.Console.Commands
{

    public class GetTodoListCommand : Command
    {
        public IConsole Console { get; set; }

        public GetTodoListCommand()
            : base(name: "get", "Gets a todo list")
        {
            this.AddArgument(new Argument<int>("id", "Id of the todo list"));
        }

        public new class Handler : ICommandHandler
        {
            private readonly IMediator meditor;

            public Handler(IMediator meditor) =>
                this.meditor = meditor ?? throw new ArgumentNullException(nameof(meditor));

            public int Id { get; set; }

            public int Invoke(InvocationContext context)
            {
                throw new NotImplementedException();
            }

            public async Task<int> InvokeAsync(InvocationContext context)
            {
                var list = await meditor.Send(new GetTodoQuery { Id = Id }).ConfigureAwait(false);
                var dump = ObjectDumper.Dump(list, DumpStyle.Console);

                var panel = new Panel(dump)
                {
                    Border = BoxBorder.Rounded,
                    Header = new PanelHeader($"{list.Title}", Justify.Right),
                };
                AnsiConsole.Render(panel);
                return 0;
            }
        }
    }
}
