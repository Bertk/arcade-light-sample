using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using RazorPagesTestSample.Pages;
using Xunit;

namespace RazorPagesTestSample.Tests.UnitTests
{
    public class PartialPageTests
    {
        [Fact]
        public void OnGetPartial_ReturnsAPartialViewResult()
        {
            // Arrange
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
            PartialsModel pageModel = new()
            {
                PageContext = pageContext,
                TempData = tempData,
                Url = new UrlHelper(actionContext),
                MetadataProvider = modelMetadataProvider
            };

            // Act
            IActionResult result = pageModel.OnGetPartial();

            // Assert
            Assert.IsType<PartialViewResult>(result);
        }
    }
}

