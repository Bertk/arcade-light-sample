using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RazorPagesTestSample.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Message> Messages { get; set; }

        #region snippet1
        public virtual async Task<List<Message>> GetMessagesAsync()
        {
            return await Messages
                .OrderBy(message => message.Text)
                .AsNoTracking()
                .ToListAsync().ConfigureAwait(false);
        }
        #endregion

        #region snippet2
        public virtual async Task AddMessageAsync(Message message)
        {
            await Messages.AddAsync(message).ConfigureAwait(false);
            await SaveChangesAsync().ConfigureAwait(false);
        }
        #endregion

        #region snippet3
        public virtual async Task DeleteAllMessagesAsync()
        {
            foreach (Message message in Messages)
            {
                Messages.Remove(message);
            }

            await SaveChangesAsync().ConfigureAwait(false);
        }
        #endregion

        #region snippet4
        public virtual async Task DeleteMessageAsync(int id)
        {
            Message message = await Messages.FindAsync(id).ConfigureAwait(false);

            if (message != null)
            {
                Messages.Remove(message);
                await SaveChangesAsync().ConfigureAwait(false);
            }
        }
        #endregion

        public void Initialize()
        {
            Messages.AddRange(GetSeedingMessages());
            SaveChanges();
        }

#pragma warning disable CA1002 // Do not expose generic lists
#pragma warning disable CA1024 // Use properties where appropriate
        public static List<Message> GetSeedingMessages()
#pragma warning restore CA1024 // Use properties where appropriate
#pragma warning restore CA1002 // Do not expose generic lists
        {
            return
            [
                new Message(){ Text = "You're standing on my scarf." },
                new Message(){ Text = "Would you like a jelly baby?" },
                new Message(){ Text = "To the rational mind, nothing is inexplicable; only unexplained." }
            ];
        }
    }
}
