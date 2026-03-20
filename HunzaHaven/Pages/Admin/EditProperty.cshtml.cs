// Pages/Admin/EditProperty.cshtml.cs - Page model for editing an existing property
// Loads the property by ID, allows updates and optional image replacement

using HunzaHaven.Data;
using HunzaHaven.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HunzaHaven.Pages.Admin
{
    /// <summary>
    /// EditPropertyModel handles loading and updating an existing property.
    /// Supports replacing the property image via file upload.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class EditPropertyModel : PageModel
    {
        // Database context for reading and updating the property
        private readonly ApplicationDbContext _context;

        // Web host environment for accessing wwwroot path
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Constructor - receives database context and environment from DI.
        /// </summary>
        public EditPropertyModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Bound property for the form data
        [BindProperty]
        public Property Property { get; set; } = new();

        // Optional file upload to replace the current image
        [BindProperty]
        public IFormFile? ImageUpload { get; set; }

        /// <summary>
        /// OnGet loads the property from the database by its ID.
        /// Returns NotFound if the property does not exist.
        /// </summary>
        /// <param name="id">The property ID from the query string</param>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Find the property in the database by ID
            var property = await _context.Properties.FindAsync(id);

            // Return 404 if not found
            if (property == null)
            {
                return NotFound();
            }

            // Populate the form with existing property data
            Property = property;
            return Page();
        }

        /// <summary>
        /// OnPostAsync saves the updated property details to the database.
        /// If a new image is uploaded, replaces the existing one.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            // Remove ImageFileName from validation as it may be set already
            ModelState.Remove("Property.ImageFileName");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Find the existing property in the database
            var existingProperty = await _context.Properties.FindAsync(Property.Id);
            if (existingProperty == null)
            {
                return NotFound();
            }

            // Update the scalar properties
            existingProperty.Name = Property.Name;
            existingProperty.Description = Property.Description;
            existingProperty.PricePerNight = Property.PricePerNight;
            existingProperty.MaxGuests = Property.MaxGuests;
            existingProperty.RoomType = Property.RoomType;
            existingProperty.IsAvailable = Property.IsAvailable;

            // ---- HANDLE IMAGE REPLACEMENT ----
            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                // Generate a unique filename for the new image
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUpload.FileName);
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save the new image file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(stream);
                }

                // Update the image filename on the property record
                existingProperty.ImageFileName = fileName;
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Set success message and redirect to admin dashboard
            TempData["Message"] = $"Property '{existingProperty.Name}' has been updated.";
            return RedirectToPage("/Admin/Admin");
        }
    }
}
