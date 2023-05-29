namespace SampleConsoleApp.Todo.Infrastructure.Services
{
    using SampleConsoleApp.Todo.Application.Common.Interfaces;
    using SampleConsoleApp.Todo.Application.Common.Models;
    using SampleConsoleApp.Todo.Domain.Common;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class DomainEventService : IDomainEventService
    {
        private readonly ILogger<DomainEventService> logger;
        private readonly IPublisher mediator;

        public DomainEventService(ILogger<DomainEventService> logger, IPublisher mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        public async Task Publish(DomainEvent domainEvent)
        {
            this.logger.LogInformation("Publishing domain event. Event - {event}", domainEvent.GetType().Name);
            await mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent)).ConfigureAwait(false);
        }

        private static INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent) =>
            (INotification)Activator
                .CreateInstance(typeof(DomainEventNotification<>)
                .MakeGenericType(domainEvent.GetType()), domainEvent);
    }
}
