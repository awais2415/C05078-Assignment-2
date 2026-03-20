// Models/ContactMessage.cs - Represents a message submitted via the contact form
// Used for server-side validation of the contact form inputs

using System.ComponentModel.DataAnnotations;

namespace HunzaHaven.Models
{
    /// <summary>
    /// ContactMessage model used for binding and validating the contact
    /// form data submitted by visitors on the Contact page.
    /// </summary>
    public class ContactMessage
    {
        // Visitor's full name - required field
        [Required(ErrorMessage = "Please enter your full name.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        // Visitor's email address - required with format validation
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        // The message body - required field
        [Required(ErrorMessage = "Please enter a message.")]
        [StringLength(2000, ErrorMessage = "Message cannot exceed 2000 characters.")]
        public string Message { get; set; } = string.Empty;

        // Optional enquiry type selection
        [Display(Name = "Enquiry Type")]
        public string? EnquiryType { get; set; }

        // Optional check-in date
        [Display(Name = "Check-in Date")]
        [DataType(DataType.Date)]
        public DateTime? CheckIn { get; set; }

        // Optional check-out date
        [Display(Name = "Check-out Date")]
        [DataType(DataType.Date)]
        public DateTime? CheckOut { get; set; }
    }
}
