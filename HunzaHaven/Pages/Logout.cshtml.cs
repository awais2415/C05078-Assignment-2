// Pages/Logout.cshtml.cs - Page model for signing out
// Removes the authentication cookie and redirects to the home page

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HunzaHaven.Pages
{
    /// <summary>
    /// LogoutModel handles the sign-out process.
    /// Clears the authentication cookie and redirects to the home page.
    /// </summary>
    public class LogoutModel : PageModel
    {
        /// <summary>
        /// OnPostAsync is called when the logout form is submitted.
        /// Signs out the user by removing the authentication cookie.
        /// </summary>
        /// <returns>Redirect to the home page</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            // Remove the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the home page after logout
            return RedirectToPage("/Index");
        }
    }
}
