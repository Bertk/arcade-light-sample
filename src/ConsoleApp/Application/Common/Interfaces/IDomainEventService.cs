namespace SampleConsoleApp.Todo.Application.Common.Interfaces
{
    using SampleConsoleApp.Todo.Domain.Common;
    using System.Threading.Tasks;

    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
