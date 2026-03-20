// Pages/Contact.cshtml.cs - Page model for the Contact page
// Handles GET display and POST form submission with server-side validation

using HunzaHaven.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HunzaHaven.Pages
{
    /// <summary>
    /// ContactModel handles the contact form submission.
    /// Validates the form data server-side and displays a success message.
    /// </summary>
    public class ContactModel : PageModel
    {
        // Bound property for the contact form data - populated from the form POST
        [BindProperty]
        public ContactMessage ContactForm { get; set; } = new();

        // Success message displayed after a valid form submission
        public string? SuccessMessage { get; set; }

        /// <summary>
        /// OnGet is called when the page loads via GET request.
        /// No special logic needed - just displays the form.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// OnPost is called when the contact form is submitted.
        /// Validates the form data and displays a success or error message.
        /// </summary>
        /// <returns>The page result with validation feedback</returns>
        public IActionResult OnPost()
        {
            // Check if the model validation passed (required fields, email format etc.)
            if (!ModelState.IsValid)
            {
                // Return the page with validation errors displayed
                return Page();
            }

            // Form is valid - display success message to the user
            SuccessMessage = $"Thank you {ContactForm.FullName}! Your message has been sent. We will reply to {ContactForm.Email} soon.";

            // Clear the form after successful submission
            ModelState.Clear();
            ContactForm = new ContactMessage();

            return Page();
        }
    }
}
