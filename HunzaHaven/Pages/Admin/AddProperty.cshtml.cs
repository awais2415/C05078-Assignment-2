// Pages/Admin/AddProperty.cshtml.cs - Page model for adding a new property
// Handles form submission including file upload for the property image

using HunzaHaven.Data;
using HunzaHaven.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HunzaHaven.Pages.Admin
{
    /// <summary>
    /// AddPropertyModel handles the creation of new property listings.
    /// Processes the form data and saves the image file to wwwroot/images.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AddPropertyModel : PageModel
    {
        // Database context for saving the new property
        private readonly ApplicationDbContext _context;

        // Web host environment for getting the wwwroot path
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Constructor - receives database context and environment from DI.
        /// </summary>
        public AddPropertyModel(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Bound property for the form data
        [BindProperty]
        public Property Property { get; set; } = new() { IsAvailable = true };

        // File upload bound from the form
        [BindProperty]
        public IFormFile? ImageUpload { get; set; }

        /// <summary>
        /// OnGet displays the empty add property form.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// OnPostAsync processes the form submission.
        /// Validates the data, saves the uploaded image and creates the database record.
        /// </summary>
        /// <returns>Redirect to admin dashboard on success, or page with errors</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            // Remove ImageFileName from validation as it is set programmatically
            ModelState.Remove("Property.ImageFileName");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // ---- HANDLE IMAGE UPLOAD ----
            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                // Generate a unique filename to avoid conflicts
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUpload.FileName);
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");

                // Create the images folder if it does not exist
                Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save the uploaded file to the wwwroot/images folder
                // Code adapted from Microsoft, 2024
                // https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(stream);
                }
                // End of adapted code

                // Set the filename on the property model
                Property.ImageFileName = fileName;
            }

            // Add the new property to the database
            _context.Properties.Add(Property);
            await _context.SaveChangesAsync();

            // Set success message and redirect to admin dashboard
            TempData["Message"] = $"Property '{Property.Name}' has been added successfully.";
            return RedirectToPage("/Admin/Admin");
        }
    }
}
