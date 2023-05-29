namespace SampleConsoleApp.Todo.Application.TodoLists.Queries.GetTodo
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using SampleConsoleApp.Todo.Application.Common.Exceptions;
    using SampleConsoleApp.Todo.Application.Common.Interfaces;
    using SampleConsoleApp.Todo.Application.TodoLists.Queries.GetTodos;
    using SampleConsoleApp.Todo.Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class GetTodoQuery : IRequest<TodoListDto>
    {
        public int Id { get; set; }
    }

    public class GetTodoQueryHandler : IRequestHandler<GetTodoQuery, TodoListDto>
    {
        private readonly IApplicationDbContext context;
        private readonly IMapper mapper;

        public GetTodoQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TodoListDto> Handle(GetTodoQuery request, CancellationToken cancellationToken)
        {
            var entity = await context.TodoLists
                .Include(t => t.Items)
                .Where(l => l.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

            if (entity == null)
            {
                throw new NotFoundException(nameof(TodoList), request.Id);
            }

            return this.mapper.Map<TodoListDto>(entity);
        }
    }
}
