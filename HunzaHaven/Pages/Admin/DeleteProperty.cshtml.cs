// Pages/Admin/DeleteProperty.cshtml.cs - Page model for deleting a property
// Loads the property for confirmation and removes it from the database on POST

using HunzaHaven.Data;
using HunzaHaven.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HunzaHaven.Pages.Admin
{
    /// <summary>
    /// DeletePropertyModel handles the property deletion confirmation page.
    /// Displays the property details on GET and deletes the record on POST.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class DeletePropertyModel : PageModel
    {
        // Database context for accessing and removing the property
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor - receives the database context from DI container.
        /// </summary>
        public DeletePropertyModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Bound property to hold the property data for display and deletion
        [BindProperty]
        public Property Property { get; set; } = new();

        /// <summary>
        /// OnGetAsync loads the property by ID for the confirmation display.
        /// </summary>
        /// <param name="id">The property ID from the query string</param>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var property = await _context.Properties.FindAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            Property = property;
            return Page();
        }

        /// <summary>
        /// OnPostAsync removes the property from the database after confirmation.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            // Find the property in the database
            var property = await _context.Properties.FindAsync(Property.Id);

            if (property != null)
            {
                // Remove the property record from the database
                _context.Properties.Remove(property);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Property '{property.Name}' has been deleted.";
            }

            return RedirectToPage("/Admin/Admin");
        }
    }
}
