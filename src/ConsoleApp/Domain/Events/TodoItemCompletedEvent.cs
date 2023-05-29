namespace SampleConsoleApp.Todo.Domain.Events
{
    using SampleConsoleApp.Todo.Domain.Common;
    using SampleConsoleApp.Todo.Domain.Entities;

    public class TodoItemCompletedEvent : DomainEvent
    {
        public TodoItemCompletedEvent(TodoItem item) => this.Item = item;

        public TodoItem Item { get; }
    }
}
