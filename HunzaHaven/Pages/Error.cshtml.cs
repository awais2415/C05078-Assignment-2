// Pages/Error.cshtml.cs - Page model for the error page
// Captures the request ID for debugging purposes

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace HunzaHaven.Pages
{
    /// <summary>
    /// ErrorModel displays a user-friendly error page when unhandled exceptions occur.
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        // The unique request ID for this error occurrence
        public string? RequestId { get; set; }

        // Whether to show the request ID (for debugging)
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// <summary>
        /// OnGet captures the current activity or trace ID.
        /// </summary>
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
