// Models/Property.cs - Represents a guesthouse property/room listing
// This model maps to the Properties table in the SQL Server database

using System.ComponentModel.DataAnnotations;

namespace HunzaHaven.Models
{
    /// <summary>
    /// Property entity storing details of each room or accommodation type
    /// offered by the Hunza Haven Guest House.
    /// </summary>
    public class Property
    {
        // Primary key - auto-incremented by the database
        public int Id { get; set; }

        // Name of the property/room type (e.g. "Standard Room", "Deluxe Suite")
        [Required(ErrorMessage = "Property name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Display(Name = "Property Name")]
        public string Name { get; set; } = string.Empty;

        // Short description of the property
        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;

        // Price per night in GBP
        [Required(ErrorMessage = "Price per night is required.")]
        [Range(1, 10000, ErrorMessage = "Price must be between £1 and £10,000.")]
        [Display(Name = "Price Per Night (£)")]
        public decimal PricePerNight { get; set; }

        // Maximum number of guests the room can accommodate
        [Required(ErrorMessage = "Maximum guests is required.")]
        [Range(1, 20, ErrorMessage = "Guests must be between 1 and 20.")]
        [Display(Name = "Max Guests")]
        public int MaxGuests { get; set; }

        // Filename of the property image stored in wwwroot/images/
        [Display(Name = "Image")]
        public string? ImageFileName { get; set; }

        // Type of room (e.g. "Standard", "Deluxe", "Family")
        [Display(Name = "Room Type")]
        public string? RoomType { get; set; }

        // Whether the property is currently available for booking
        [Display(Name = "Available")]
        public bool IsAvailable { get; set; } = true;
    }
}
