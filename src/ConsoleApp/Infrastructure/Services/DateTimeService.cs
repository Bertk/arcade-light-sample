namespace SampleConsoleApp.Todo.Infrastructure.Services
{
    using SampleConsoleApp.Todo.Application.Common.Interfaces;
    using System;

    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
