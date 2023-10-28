using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesIntro.Pages
{
    public class PrivacyModel : PageModel
    {
#pragma warning disable S4487 // Unread "private" fields should be removed
        private readonly ILogger<PrivacyModel> _logger;
#pragma warning restore S4487 // Unread "private" fields should be removed

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // to be populated
        }
    }
}
