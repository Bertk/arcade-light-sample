namespace SampleConsoleApp.Todo.Domain.Entities
{
    using SampleConsoleApp.Todo.Domain.Common;
    using SampleConsoleApp.Todo.Domain.Enums;
    using SampleConsoleApp.Todo.Domain.Events;
    using System;
    using System.Collections.Generic;

    public class TodoItem : AuditableEntity, IHasDomainEvent
    {
        public int Id { get; set; }

        public TodoList List { get; set; }

        public int ListId { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public PriorityLevel Priority { get; set; }

        public DateTime? Reminder { get; set; }

        private bool done;
        public bool Done
        {
            get => this.done;
            set
            {
                if (value == true && this.done == false)
                {
                    this.DomainEvents.Add(new TodoItemCompletedEvent(this));
                }

                this.done = value;
            }
        }

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
