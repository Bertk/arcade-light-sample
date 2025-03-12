using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorPagesTestSample.Data;
using RazorPagesTestSample.Tests.Utilities;
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
            await db.AddRangeAsync(expectedMessages, TestContext.Current.CancellationToken);
            _ = await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            // Act
            List<Message> result = await db.GetMessagesAsync();

            // Assert
            List<Message> actualMessages = Assert.IsType<List<Message>>(result, exactMatch: false);
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
            Message actualMessage = await db.FindAsync<Message>(new object[] { recId, TestContext.Current.CancellationToken }, TestContext.Current.CancellationToken);
            Assert.Equal(expectedMessage, actualMessage);
        }

        [Fact]
        public async Task DeleteAllMessagesAsync_MessagesAreDeleted()
        {
            using AppDbContext db = new(TestUtilities.TestDbContextOptions());
            // Arrange
            List<Message> seedMessages = AppDbContext.GetSeedingMessages();
            await db.AddRangeAsync(seedMessages, TestContext.Current.CancellationToken);
            _ = await db.SaveChangesAsync(TestContext.Current.CancellationToken);

            // Act
            await db.DeleteAllMessagesAsync();

            // Assert
            Assert.Empty(await db.Messages.AsNoTracking().ToListAsync(cancellationToken: TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task DeleteMessageAsync_MessageIsDeleted_WhenMessageIsFound()
        {
            using AppDbContext db = new(TestUtilities.TestDbContextOptions());
            #region snippet1
            // Arrange
            List<Message> seedMessages = AppDbContext.GetSeedingMessages();
            await db.AddRangeAsync(seedMessages, TestContext.Current.CancellationToken);
            _ = await db.SaveChangesAsync(TestContext.Current.CancellationToken);
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
            List<Message> actualMessages = await db.Messages.AsNoTracking().ToListAsync(cancellationToken: TestContext.Current.CancellationToken);
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
            await db.AddRangeAsync(expectedMessages, TestContext.Current.CancellationToken);
            _ = await db.SaveChangesAsync(TestContext.Current.CancellationToken);
            int recId = 4;

            // Act
            try
            {
                await db.DeleteMessageAsync(recId);
            }
            catch (DbUpdateConcurrencyException)
            {
                // recId doesn't exist
            }

            // Assert
            List<Message> actualMessages = await db.Messages.AsNoTracking().ToListAsync(cancellationToken: TestContext.Current.CancellationToken);
            Assert.Equal(
                expectedMessages.OrderBy(m => m.Id).Select(m => m.Text),
                actualMessages.OrderBy(m => m.Id).Select(m => m.Text));
        }
        #endregion
    }
}
