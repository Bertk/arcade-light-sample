namespace SampleConsoleApp.Todo.Application.Common.Models
{
    using SampleConsoleApp.Todo.Domain.Common;
    using MediatR;

    public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : DomainEvent
    {
        public DomainEventNotification(TDomainEvent domainEvent) => this.DomainEvent = domainEvent;

        public TDomainEvent DomainEvent { get; }
    }
}
