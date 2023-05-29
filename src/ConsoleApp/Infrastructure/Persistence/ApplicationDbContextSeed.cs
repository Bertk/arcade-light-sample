namespace SampleConsoleApp.Todo.Infrastructure.Persistence
{
    using SampleConsoleApp.Todo.Domain.Entities;
    using SampleConsoleApp.Todo.Domain.ValueObjects;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ApplicationDbContextSeed
    {

        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            if (!context.TodoLists.Any())
            {
                context.TodoLists.Add(new TodoList
                {
                    Title = "Shopping",
                    Colour = Colour.Blue,
                    Items =
                    {
                        new TodoItem { Title = "Milk", Done = true },
                        new TodoItem { Title = "Bread", Done = true },
                        new TodoItem { Title = "Pasta" },
                        new TodoItem { Title = "Water" }
                    }
                });

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
