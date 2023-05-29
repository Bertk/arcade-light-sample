namespace SampleConsoleApp.Todo.Application.TodoLists.Commands.PurgeTodoLists
{
    using SampleConsoleApp.Todo.Application.Common.Interfaces;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class PurgeTodoListsCommand : IRequest
    {
    }

    public class PurgeTodoListsCommandHandler : IRequestHandler<PurgeTodoListsCommand>
    {
        private readonly IApplicationDbContext context;

        public PurgeTodoListsCommandHandler(IApplicationDbContext context) => this.context = context;

        public async Task<Unit> Handle(PurgeTodoListsCommand request, CancellationToken cancellationToken)
        {
            this.context.TodoLists.RemoveRange(this.context.TodoLists);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return Unit.Value;
        }

        Task IRequestHandler<PurgeTodoListsCommand>.Handle(PurgeTodoListsCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
