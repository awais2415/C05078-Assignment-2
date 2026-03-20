// Models/AdminUser.cs - Represents an admin user who can manage the site
// This model maps to the AdminUsers table in the SQL Server database

using System.ComponentModel.DataAnnotations;

namespace HunzaHaven.Models
{
    /// <summary>
    /// AdminUser entity storing login credentials for site administrators.
    /// Passwords are hashed using BCrypt for security.
    /// </summary>
    public class AdminUser
    {
        // Primary key - auto-incremented by the database
        public int Id { get; set; }

        // Admin email used as the username for login
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        // Hashed password stored securely in the database
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Display name shown in the admin area
        [StringLength(100)]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; } = string.Empty;
    }
}
