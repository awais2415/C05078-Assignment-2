// Pages/Index.cshtml.cs - Page model for the Home page
// Retrieves available property listings from the database to display on the home page

using HunzaHaven.Data;
using HunzaHaven.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HunzaHaven.Pages
{
    /// <summary>
    /// IndexModel handles the GET request for the home page.
    /// It loads available properties from the database to display as room cards.
    /// </summary>
    public class IndexModel : PageModel
    {
        // Database context injected via dependency injection
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor - receives the database context from DI container.
        /// </summary>
        /// <param name="context">The application database context</param>
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // List of properties to display on the page
        public List<Property> Properties { get; set; } = new();

        /// <summary>
        /// OnGetAsync is called when the page receives a GET request.
        /// Loads all available properties from the database.
        /// </summary>
        public async Task OnGetAsync()
        {
            // Query the database for available properties only
            Properties = await _context.Properties
                .Where(p => p.IsAvailable)
                .ToListAsync();
        }
    }
}
