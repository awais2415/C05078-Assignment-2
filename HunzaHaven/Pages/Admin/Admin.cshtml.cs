// Pages/Admin/Admin.cshtml.cs - Page model for the Admin dashboard
// Loads all properties and gallery images for the admin to manage

using HunzaHaven.Data;
using HunzaHaven.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HunzaHaven.Pages.Admin
{
    /// <summary>
    /// AdminModel handles the admin dashboard page.
    /// Restricted to users in the "Admin" role via the Authorize attribute.
    /// Loads all properties and gallery images from the database.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminModel : PageModel
    {
        // Database context injected via dependency injection
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor - receives the database context from DI container.
        /// </summary>
        /// <param name="context">The application database context</param>
        public AdminModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // List of all properties displayed in the admin table
        public List<Property> Properties { get; set; } = new();

        // List of all gallery images displayed in the admin table
        public List<GalleryImage> GalleryImages { get; set; } = new();

        // Optional message displayed after an action (e.g. "Property deleted")
        [TempData]
        public string? Message { get; set; }

        /// <summary>
        /// OnGetAsync loads all properties and gallery images from the database.
        /// </summary>
        public async Task OnGetAsync()
        {
            // Load all properties ordered by name
            Properties = await _context.Properties
                .OrderBy(p => p.Name)
                .ToListAsync();

            // Load all gallery images ordered by display order
            GalleryImages = await _context.GalleryImages
                .OrderBy(g => g.DisplayOrder)
                .ToListAsync();
        }
    }
}
