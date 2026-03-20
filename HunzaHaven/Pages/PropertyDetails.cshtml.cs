// Pages/PropertyDetails.cshtml.cs - Page model for the Property Details page
// Retrieves all properties from the database to display as detailed cards

using HunzaHaven.Data;
using HunzaHaven.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HunzaHaven.Pages
{
    /// <summary>
    /// PropertyDetailsModel handles the GET request for the property listings page.
    /// Loads all properties from the database ordered by price.
    /// </summary>
    public class PropertyDetailsModel : PageModel
    {
        // Database context injected via dependency injection
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor - receives the database context from DI container.
        /// </summary>
        /// <param name="context">The application database context</param>
        public PropertyDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // List of all properties to display on the page
        public List<Property> Properties { get; set; } = new();

        /// <summary>
        /// OnGetAsync is called when the page receives a GET request.
        /// Loads all properties ordered by price ascending.
        /// </summary>
        public async Task OnGetAsync()
        {
            // Query all properties from database, ordered by price (cheapest first)
            Properties = await _context.Properties
                .OrderBy(p => p.PricePerNight)
                .ToListAsync();
        }
    }
}
