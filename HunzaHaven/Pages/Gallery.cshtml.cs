// Pages/Gallery.cshtml.cs - Page model for the Gallery page
// Retrieves gallery images from the database ordered by display order

using HunzaHaven.Data;
using HunzaHaven.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HunzaHaven.Pages
{
    /// <summary>
    /// GalleryModel handles the GET request for the gallery page.
    /// Loads all gallery images from the database to display in a carousel and grid.
    /// </summary>
    public class GalleryModel : PageModel
    {
        // Database context injected via dependency injection
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor - receives the database context from DI container.
        /// </summary>
        /// <param name="context">The application database context</param>
        public GalleryModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // List of gallery images to display on the page
        public List<GalleryImage> GalleryImages { get; set; } = new();

        /// <summary>
        /// OnGetAsync is called when the page receives a GET request.
        /// Loads all gallery images ordered by their display order.
        /// </summary>
        public async Task OnGetAsync()
        {
            // Query the database for all gallery images, sorted by display order
            GalleryImages = await _context.GalleryImages
                .OrderBy(g => g.DisplayOrder)
                .ToListAsync();
        }
    }
}
