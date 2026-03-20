// Pages/Login.cshtml.cs - Page model for the Login page
// Handles admin authentication using cookie-based login
// Code adapted from Microsoft, 2024
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie

using HunzaHaven.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HunzaHaven.Pages
{
    /// <summary>
    /// LoginModel handles the admin login form.
    /// Validates credentials against the database and creates an authentication cookie.
    /// </summary>
    public class LoginModel : PageModel
    {
        // Database context injected via dependency injection
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor - receives the database context from DI container.
        /// </summary>
        /// <param name="context">The application database context</param>
        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Email input bound from the login form
        [BindProperty]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        // Password input bound from the login form
        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        // Error message displayed if login fails
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// OnGet is called when the page loads via GET request.
        /// Redirects to admin area if already logged in.
        /// </summary>
        public IActionResult OnGet()
        {
            // If user is already authenticated, redirect to admin page
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Admin/Admin");
            }
            return Page();
        }

        /// <summary>
        /// OnPostAsync is called when the login form is submitted.
        /// Validates credentials and creates an authentication cookie if successful.
        /// </summary>
        /// <returns>Redirect to admin area on success, or page with error on failure</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            // Check form validation (required fields, email format)
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Hash the submitted password to compare against the stored hash
            var hashedPassword = DbInitialiser.HashPassword(Password);

            // Look up the admin user by email and password hash
            var adminUser = await _context.AdminUsers
                .FirstOrDefaultAsync(u => u.Email == Email && u.PasswordHash == hashedPassword);

            // If no matching user found, show error message
            if (adminUser == null)
            {
                ErrorMessage = "Invalid email or password. Please try again.";
                return Page();
            }

            // ---- CREATE AUTHENTICATION COOKIE ----
            // Build the claims for the authenticated user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, adminUser.DisplayName),
                new Claim(ClaimTypes.Email, adminUser.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            // Create the claims identity with cookie authentication scheme
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Set authentication properties (persistent cookie, 30 minute expiry)
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            // Sign in the user by creating the authentication cookie
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Redirect to the admin dashboard
            return RedirectToPage("/Admin/Admin");
        }
    }
}
// End of adapted code
