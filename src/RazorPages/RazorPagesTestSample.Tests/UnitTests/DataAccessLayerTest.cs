using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorPagesTestSample.Data;
using Xunit;

namespace RazorPagesTestSample.Tests.UnitTests
{
    public class DataAccessLayerTest
    {
        [Fact]
        public async Task GetMessagesAsync_MessagesAreReturned()
        {
            using AppDbContext db = new(TestUtilities.TestDbContextOptions());
            // Arrange
            List<Message> expectedMessages = AppDbContext.GetSeedingMessages();
            await db.AddRangeAsync(expectedMessages);
            await db.SaveChangesAsync();

            // Act
            List<Message> result = await db.GetMessagesAsync();

            // Assert
            List<Message> actualMessages = Assert.IsAssignableFrom<List<Message>>(result);
            Assert.Equal(
                expectedMessages.OrderBy(m => m.Id).Select(m => m.Text),
                actualMessages.OrderBy(m => m.Id).Select(m => m.Text));
        }

        [Fact]
        public async Task AddMessageAsync_MessageIsAdded()
        {
            using AppDbContext db = new(TestUtilities.TestDbContextOptions());
            // Arrange
            int recId = 10;
            Message expectedMessage = new() { Id = recId, Text = "Message" };

            // Act
            await db.AddMessageAsync(expectedMessage);

            // Assert
            Message actualMessage = await db.FindAsync<Message>(recId);
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public async Task DeleteAllMessagesAsync_MessagesAreDeleted()
        {
            using AppDbContext db = new(TestUtilities.TestDbContextOptions());
            // Arrange
            List<Message> seedMessages = AppDbContext.GetSeedingMessages();
            await db.AddRangeAsync(seedMessages);
            await db.SaveChangesAsync();

            // Act
            await db.DeleteAllMessagesAsync();

            // Assert
            Assert.Empty(await db.Messages.AsNoTracking().ToListAsync());
        }

        [Fact]
        public async Task DeleteMessageAsync_MessageIsDeleted_WhenMessageIsFound()
        {
            using AppDbContext db = new(TestUtilities.TestDbContextOptions());
            #region snippet1
            // Arrange
            List<Message> seedMessages = AppDbContext.GetSeedingMessages();
            await db.AddRangeAsync(seedMessages);
            await db.SaveChangesAsync();
            int recId = 1;
            List<Message> expectedMessages =
                seedMessages.Where(message => message.Id != recId).ToList();
            #endregion

            #region snippet2
            // Act
            await db.DeleteMessageAsync(recId);
            #endregion

            #region snippet3
            // Assert
            List<Message> actualMessages = await db.Messages.AsNoTracking().ToListAsync();
            Assert.Equal(
                expectedMessages.OrderBy(m => m.Id).Select(m => m.Text),
                actualMessages.OrderBy(m => m.Id).Select(m => m.Text));
            #endregion
        }

        #region snippet4
        [Fact]
        public async Task DeleteMessageAsync_NoMessageIsDeleted_WhenMessageIsNotFound()
        {
            using AppDbContext db = new(TestUtilities.TestDbContextOptions());
            // Arrange
            List<Message> expectedMessages = AppDbContext.GetSeedingMessages();
            await db.AddRangeAsync(expectedMessages);
            await db.SaveChangesAsync();
            int recId = 4;

            // Act
#pragma warning disable CA1031 // Do not catch general exception types
            try
            {
                await db.DeleteMessageAsync(recId);
            }
            catch
            {
                // recId doesn't exist
            }
#pragma warning restore CA1031 // Do not catch general exception types

            // Assert
            List<Message> actualMessages = await db.Messages.AsNoTracking().ToListAsync();
            Assert.Equal(
                expectedMessages.OrderBy(m => m.Id).Select(m => m.Text),
                actualMessages.OrderBy(m => m.Id).Select(m => m.Text));
        }
        #endregion
    }
}
