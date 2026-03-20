// Pages/Admin/DeleteGalleryImage.cshtml.cs - Page model for deleting a gallery image
// Loads the image for confirmation and removes it from the database on POST

using HunzaHaven.Data;
using HunzaHaven.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HunzaHaven.Pages.Admin
{
    /// <summary>
    /// DeleteGalleryImageModel handles the gallery image deletion process.
    /// Displays image details on GET and deletes the record on POST.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class DeleteGalleryImageModel : PageModel
    {
        // Database context for accessing and removing the gallery image
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor - receives the database context from DI container.
        /// </summary>
        public DeleteGalleryImageModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Bound property for the gallery image data
        [BindProperty]
        public GalleryImage GalleryImage { get; set; } = new();

        /// <summary>
        /// OnGetAsync loads the gallery image by ID for the confirmation display.
        /// </summary>
        /// <param name="id">The gallery image ID from the query string</param>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var galleryImage = await _context.GalleryImages.FindAsync(id);

            if (galleryImage == null)
            {
                return NotFound();
            }

            GalleryImage = galleryImage;
            return Page();
        }

        /// <summary>
        /// OnPostAsync removes the gallery image from the database.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            var galleryImage = await _context.GalleryImages.FindAsync(GalleryImage.Id);

            if (galleryImage != null)
            {
                // Remove the gallery image record from the database
                _context.GalleryImages.Remove(galleryImage);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Gallery image '{galleryImage.Caption}' has been deleted.";
            }

            return RedirectToPage("/Admin/Admin");
        }
    }
}
