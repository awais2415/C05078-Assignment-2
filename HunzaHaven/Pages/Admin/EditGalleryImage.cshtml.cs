// Pages/Admin/EditGalleryImage.cshtml.cs - Page model for editing a gallery image
// Loads the image by ID and supports updating caption, alt text and image file

using HunzaHaven.Data;
using HunzaHaven.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HunzaHaven.Pages.Admin
{
    /// <summary>
    /// EditGalleryImageModel handles loading and updating an existing gallery image.
    /// Supports replacing the image file via upload.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class EditGalleryImageModel : PageModel
    {
        // Database context for reading and updating the gallery image
        private readonly ApplicationDbContext _context;

        // Web host environment for accessing wwwroot path
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Constructor - receives database context and environment from DI.
        /// </summary>
        public EditGalleryImageModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Bound property for the gallery image form data
        [BindProperty]
        public GalleryImage GalleryImage { get; set; } = new();

        // Optional file upload to replace the current image
        [BindProperty]
        public IFormFile? ImageUpload { get; set; }

        /// <summary>
        /// OnGetAsync loads the gallery image from the database by its ID.
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
        /// OnPostAsync saves the updated gallery image details to the database.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            // Remove FileName from validation as it may already be set
            ModelState.Remove("GalleryImage.FileName");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Find the existing gallery image in the database
            var existingImage = await _context.GalleryImages.FindAsync(GalleryImage.Id);
            if (existingImage == null)
            {
                return NotFound();
            }

            // Update the text fields
            existingImage.Caption = GalleryImage.Caption;
            existingImage.AltText = GalleryImage.AltText;
            existingImage.DisplayOrder = GalleryImage.DisplayOrder;

            // Handle image replacement if a new file was uploaded
            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUpload.FileName);
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(stream);
                }

                existingImage.FileName = fileName;
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Gallery image '{existingImage.Caption}' has been updated.";
            return RedirectToPage("/Admin/Admin");
        }
    }
}
