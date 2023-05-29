namespace SampleConsoleApp.Todo.Domain.Entities
{
    using SampleConsoleApp.Todo.Domain.Common;
    using SampleConsoleApp.Todo.Domain.ValueObjects;
    using System.Collections.Generic;

    public class TodoList : AuditableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Colour Colour { get; set; } = Colour.White;

        public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
    }
}
