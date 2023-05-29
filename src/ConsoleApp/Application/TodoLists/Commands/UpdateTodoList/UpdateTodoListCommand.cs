namespace SampleConsoleApp.Todo.Application.TodoLists.Commands.UpdateTodoList
{
    using SampleConsoleApp.Todo.Application.Common.Exceptions;
    using SampleConsoleApp.Todo.Application.Common.Interfaces;
    using SampleConsoleApp.Todo.Domain.Entities;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateTodoListCommand : IRequest
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand>
    {
        private readonly IApplicationDbContext context;

        public UpdateTodoListCommandHandler(IApplicationDbContext context) => this.context = context;

        public async Task<Unit> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.TodoLists.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken).ConfigureAwait(false);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoList), request.Id);
            }

            entity.Title = request.Title;

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return Unit.Value;
        }

        Task IRequestHandler<UpdateTodoListCommand>.Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
