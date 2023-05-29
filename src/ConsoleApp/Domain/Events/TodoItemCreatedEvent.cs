namespace SampleConsoleApp.Todo.Domain.Events
{
    using SampleConsoleApp.Todo.Domain.Common;
    using SampleConsoleApp.Todo.Domain.Entities;

    public class TodoItemCreatedEvent : DomainEvent
    {
        public TodoItemCreatedEvent(TodoItem item) => this.Item = item;

        public TodoItem Item { get; }
    }
}
