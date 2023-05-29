namespace SampleConsoleApp.Todo.Application.TodoLists.Queries.GetTodos
{
    using SampleConsoleApp.Todo.Application.Common.Mappings;
    using SampleConsoleApp.Todo.Domain.Entities;
    using System.Collections.Generic;

    public class TodoListDto : IMapFrom<TodoList>
    {
        public TodoListDto() => this.Items = new List<TodoItemDto>();

        public int Id { get; set; }

        public string Title { get; set; }

        public string Colour { get; set; }

        public IList<TodoItemDto> Items { get; set; }
    }
}
