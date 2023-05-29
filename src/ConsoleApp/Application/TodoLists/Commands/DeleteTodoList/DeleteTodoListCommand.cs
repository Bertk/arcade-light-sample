namespace SampleConsoleApp.Todo.Application.TodoLists.Commands.DeleteTodoList
{
    using SampleConsoleApp.Todo.Application.Common.Exceptions;
    using SampleConsoleApp.Todo.Application.Common.Interfaces;
    using SampleConsoleApp.Todo.Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class DeleteTodoListCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand>
    {
        private readonly IApplicationDbContext context;

        public DeleteTodoListCommandHandler(IApplicationDbContext context) => this.context = context;

        public async Task<Unit> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.TodoLists
                .Where(l => l.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoList), request.Id);
            }

            this.context.TodoLists.Remove(entity);

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return Unit.Value;
        }

        Task IRequestHandler<DeleteTodoListCommand>.Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
