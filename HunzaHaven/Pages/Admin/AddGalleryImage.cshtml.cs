// Pages/Admin/AddGalleryImage.cshtml.cs - Page model for adding a gallery image
// Handles image file upload and creates a new GalleryImage database record

using HunzaHaven.Data;
using HunzaHaven.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HunzaHaven.Pages.Admin
{
    /// <summary>
    /// AddGalleryImageModel handles the creation of new gallery images.
    /// Processes the uploaded image file and saves it to wwwroot/images.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AddGalleryImageModel : PageModel
    {
        // Database context for saving the new gallery image record
        private readonly ApplicationDbContext _context;

        // Web host environment for accessing the wwwroot path
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Constructor - receives database context and environment from DI.
        /// </summary>
        public AddGalleryImageModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Bound property for the gallery image form data
        [BindProperty]
        public GalleryImage GalleryImage { get; set; } = new();

        // File upload bound from the form
        [BindProperty]
        public IFormFile? ImageUpload { get; set; }

        /// <summary>
        /// OnGet displays the empty add gallery image form.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// OnPostAsync processes the form submission.
        /// Validates data, saves the uploaded image and creates the database record.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            // Remove FileName from validation as it is set programmatically
            ModelState.Remove("GalleryImage.FileName");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Ensure an image file was uploaded
            if (ImageUpload == null || ImageUpload.Length == 0)
            {
                ModelState.AddModelError("ImageUpload", "Please upload an image file.");
                return Page();
            }

            // Generate a unique filename and save the image
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUpload.FileName);
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
            Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save the uploaded file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageUpload.CopyToAsync(stream);
            }

            // Set the filename on the gallery image model
            GalleryImage.FileName = fileName;

            // Add the new gallery image to the database
            _context.GalleryImages.Add(GalleryImage);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Gallery image '{GalleryImage.Caption}' has been added.";
            return RedirectToPage("/Admin/Admin");
        }
    }
}
