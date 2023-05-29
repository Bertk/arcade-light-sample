namespace SampleConsoleApp.Todo.Application.TodoItems.EventHandlers
{
    using SampleConsoleApp.Todo.Application.Common.Models;
    using SampleConsoleApp.Todo.Domain.Events;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class TodoItemCompletedHandler : INotificationHandler<DomainEventNotification<TodoItemCompletedEvent>>
    {
        private readonly ILogger<TodoItemCompletedHandler> logger;

        public TodoItemCompletedHandler(ILogger<TodoItemCompletedHandler> logger) => this.logger = logger;

        public Task Handle(DomainEventNotification<TodoItemCompletedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            this.logger.LogInformation("CleanArchitectureJT.Example Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }
    }
}
