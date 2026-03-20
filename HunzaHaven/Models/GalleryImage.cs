// Models/GalleryImage.cs - Represents an image in the guesthouse gallery
// This model maps to the GalleryImages table in the SQL Server database

using System.ComponentModel.DataAnnotations;

namespace HunzaHaven.Models
{
    /// <summary>
    /// GalleryImage entity storing details of each photo displayed
    /// in the public gallery and admin-managed carousel.
    /// </summary>
    public class GalleryImage
    {
        // Primary key - auto-incremented by the database
        public int Id { get; set; }

        // Descriptive caption shown below the image
        [Required(ErrorMessage = "Caption is required.")]
        [StringLength(200, ErrorMessage = "Caption cannot exceed 200 characters.")]
        public string Caption { get; set; } = string.Empty;

        // Alt text for accessibility (used in the img alt attribute)
        [Required(ErrorMessage = "Alt text is required for accessibility.")]
        [StringLength(200, ErrorMessage = "Alt text cannot exceed 200 characters.")]
        [Display(Name = "Alt Text")]
        public string AltText { get; set; } = string.Empty;

        // Filename of the image stored in wwwroot/images/
        [Required(ErrorMessage = "Image file is required.")]
        [Display(Name = "Image File")]
        public string FileName { get; set; } = string.Empty;

        // Display order in the gallery (lower numbers appear first)
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
    }
}
