namespace SampleConsoleApp.Todo.Application.TodoItems.Commands.UpdateTodoItemDetail
{
    using SampleConsoleApp.Todo.Application.Common.Exceptions;
    using SampleConsoleApp.Todo.Application.Common.Interfaces;
    using SampleConsoleApp.Todo.Domain.Entities;
    using SampleConsoleApp.Todo.Domain.Enums;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateTodoItemDetailCommand : IRequest
    {
        public int Id { get; set; }

        public int ListId { get; set; }

        public PriorityLevel Priority { get; set; }

        public string Note { get; set; }
    }

    public class UpdateTodoItemDetailCommandHandler : IRequestHandler<UpdateTodoItemDetailCommand>
    {
        private readonly IApplicationDbContext context;

        public UpdateTodoItemDetailCommandHandler(IApplicationDbContext context) => this.context = context;

        public async Task<Unit> Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
        {
            var entity = await context.TodoItems.FindAsync(
                new object[] { request.Id }, cancellationToken: cancellationToken).ConfigureAwait(false);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoItem), request.Id);
            }

            entity.ListId = request.ListId;
            entity.Priority = request.Priority;
            entity.Note = request.Note;

            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return Unit.Value;
        }

        Task IRequestHandler<UpdateTodoItemDetailCommand>.Handle(UpdateTodoItemDetailCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
