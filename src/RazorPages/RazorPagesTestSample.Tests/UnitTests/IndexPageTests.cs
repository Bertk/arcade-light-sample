using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Moq;
using RazorPagesTestSample.Data;
using RazorPagesTestSample.Pages;
using Xunit;

namespace RazorPagesTestSample.Tests.UnitTests
{
    public class IndexPageTests
    {
        [Fact]
        public async Task OnGetAsync_PopulatesThePageModel_WithAListOfMessages()
        {
            // Arrange
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            #region snippet1
            Mock<AppDbContext> mockAppDbContext = new(optionsBuilder.Options);
            List<Message> expectedMessages = AppDbContext.GetSeedingMessages();
            mockAppDbContext.Setup(
                db => db.GetMessagesAsync()).Returns(Task.FromResult(expectedMessages));
            IndexModel pageModel = new(mockAppDbContext.Object);
            #endregion

            #region snippet2
            // Act
            await pageModel.OnGetAsync();
            #endregion

            #region snippet3
            // Assert
            List<Message> actualMessages = Assert.IsAssignableFrom<List<Message>>(pageModel.Messages);
            Assert.Equal(
                expectedMessages.OrderBy(m => m.Id).Select(m => m.Text),
                actualMessages.OrderBy(m => m.Id).Select(m => m.Text));
            #endregion
        }

        #region snippet4
        [Fact]
        public async Task OnPostAddMessageAsync_ReturnsAPageResult_WhenModelStateIsInvalid()
        {
            // Arrange
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            Mock<AppDbContext> mockAppDbContext = new(optionsBuilder.Options);
            List<Message> expectedMessages = AppDbContext.GetSeedingMessages();
            mockAppDbContext.Setup(db => db.GetMessagesAsync()).Returns(Task.FromResult(expectedMessages));
            DefaultHttpContext httpContext = new();
            ModelStateDictionary modelState = new();
            ActionContext actionContext = new(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            EmptyModelMetadataProvider modelMetadataProvider = new();
            ViewDataDictionary viewData = new(modelMetadataProvider, modelState);
            TempDataDictionary tempData = new(httpContext, Mock.Of<ITempDataProvider>());
            PageContext pageContext = new(actionContext)
            {
                ViewData = viewData
            };
            IndexModel pageModel = new(mockAppDbContext.Object)
            {
                PageContext = pageContext,
                TempData = tempData,
                Url = new UrlHelper(actionContext)
            };
            pageModel.ModelState.AddModelError("Message.Text", "The Text field is required.");

            // Act
            IActionResult result = await pageModel.OnPostAddMessageAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }
        #endregion

        [Fact]
        public async Task OnPostAddMessageAsync_ReturnsARedirectToPageResult_WhenModelStateIsValid()
        {
            // Arrange
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            Mock<AppDbContext> mockAppDbContext = new(optionsBuilder.Options);
            List<Message> expectedMessages = AppDbContext.GetSeedingMessages();
            mockAppDbContext.Setup(db => db.GetMessagesAsync()).Returns(Task.FromResult(expectedMessages));
            DefaultHttpContext httpContext = new();
            ModelStateDictionary modelState = new();
            ActionContext actionContext = new(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            EmptyModelMetadataProvider modelMetadataProvider = new();
            ViewDataDictionary viewData = new(modelMetadataProvider, modelState);
            TempDataDictionary tempData = new(httpContext, Mock.Of<ITempDataProvider>());
            PageContext pageContext = new(actionContext)
            {
                ViewData = viewData
            };
            IndexModel pageModel = new(mockAppDbContext.Object)
            {
                PageContext = pageContext,
                TempData = tempData,
                Url = new UrlHelper(actionContext)
            };

            // Act
            // A new ModelStateDictionary is valid by default.
            IActionResult result = await pageModel.OnPostAddMessageAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task OnPostDeleteAllMessagesAsync_ReturnsARedirectToPageResult()
        {
            // Arrange
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            Mock<AppDbContext> mockAppDbContext = new(optionsBuilder.Options);
            IndexModel pageModel = new(mockAppDbContext.Object);

            // Act
            IActionResult result = await pageModel.OnPostDeleteAllMessagesAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task OnPostDeleteMessageAsync_ReturnsARedirectToPageResult()
        {
            // Arrange
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            Mock<AppDbContext> mockAppDbContext = new(optionsBuilder.Options);
            IndexModel pageModel = new(mockAppDbContext.Object);
            int recId = 1;

            // Act
            IActionResult result = await pageModel.OnPostDeleteMessageAsync(recId);

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task OnPostAnalyzeMessagesAsync_ReturnsARedirectToPageResultWithCorrectAnalysis_WhenMessagesArePresent()
        {
            // Arrange
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            Mock<AppDbContext> mockAppDbContext = new(optionsBuilder.Options);
            List<Message> seedMessages = AppDbContext.GetSeedingMessages();
            mockAppDbContext.Setup(db => db.GetMessagesAsync()).Returns(Task.FromResult(seedMessages));
            IndexModel pageModel = new(mockAppDbContext.Object);
            int wordCount = 0;

            foreach (Message message in seedMessages)
            {
                wordCount += message.Text.Split(' ').Length;
            }

            decimal avgWordCount = Decimal.Divide(wordCount, seedMessages.Count);
            string expectedMessageAnalysisResultString = $"The average message length is {avgWordCount:0.##} words.";

            // Act
            IActionResult result = await pageModel.OnPostAnalyzeMessagesAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal(expectedMessageAnalysisResultString, pageModel.MessageAnalysisResult);
        }

        [Fact]
        public async Task OnPostAnalyzeMessagesAsync_ReturnsARedirectToPageResultWithCorrectAnalysis_WhenNoMessagesArePresent()
        {
            // Arrange
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("InMemoryDb");
            Mock<AppDbContext> mockAppDbContext = new(optionsBuilder.Options);
            mockAppDbContext.Setup(db => db.GetMessagesAsync()).Returns(Task.FromResult(new List<Message>()));
            IndexModel pageModel = new(mockAppDbContext.Object);
            string expectedMessageAnalysisResultString = "There are no messages to analyze.";

            // Act
            IActionResult result = await pageModel.OnPostAnalyzeMessagesAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal(expectedMessageAnalysisResultString, pageModel.MessageAnalysisResult);
        }
    }
}
