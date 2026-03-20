// Data/DbInitialiser.cs - Seeds the database with initial data on first run
// Creates default admin user, sample properties and gallery images

using HunzaHaven.Models;

namespace HunzaHaven.Data
{
    /// <summary>
    /// DbInitialiser provides a static Seed method that populates the database
    /// with initial data if the tables are empty. This runs on application startup.
    /// </summary>
    public static class DbInitialiser
    {
        /// <summary>
        /// Seeds the database with default admin credentials, property listings
        /// and gallery images. Only inserts data if the tables are currently empty.
        /// </summary>
        /// <param name="context">The application database context</param>
        public static void Seed(ApplicationDbContext context)
        {
            // ---- SEED ADMIN USER ----
            // Only add the default admin if no admin users exist yet
            if (!context.AdminUsers.Any())
            {
                // Password "P@55word" is hashed for secure storage
                // Using simple hash for demonstration - production would use BCrypt
                context.AdminUsers.Add(new AdminUser
                {
                    Email = "J119134@chester.ac.uk",
                    PasswordHash = HashPassword("P@55word"),
                    DisplayName = "Admin"
                });
                context.SaveChanges(); // Persist admin user to database
            }

            // ---- SEED PROPERTIES ----
            // Only add sample properties if the table is empty
            if (!context.Properties.Any())
            {
                var properties = new List<Property>
                {
                    new Property
                    {
                        Name = "Standard Room",
                        Description = "Comfortable double room with private balcony overlooking the Karakoram mountain range. Includes traditional wooden furniture, en-suite bathroom and complimentary breakfast.",
                        PricePerNight = 45.00m,
                        MaxGuests = 2,
                        ImageFileName = "image5.jpg",
                        RoomType = "Standard",
                        IsAvailable = true
                    },
                    new Property
                    {
                        Name = "Deluxe Suite",
                        Description = "Spacious suite with separate seating area and panoramic mountain views. Features handcrafted Hunzai decor, king-size bed and premium amenities.",
                        PricePerNight = 75.00m,
                        MaxGuests = 3,
                        ImageFileName = "image3.jpg",
                        RoomType = "Deluxe",
                        IsAvailable = true
                    },
                    new Property
                    {
                        Name = "Family Room",
                        Description = "Large room suitable for families with children. Two double beds, private bathroom and a small kitchenette area. Garden view with mountain backdrop.",
                        PricePerNight = 95.00m,
                        MaxGuests = 5,
                        ImageFileName = "image4.jpg",
                        RoomType = "Family",
                        IsAvailable = true
                    },
                    new Property
                    {
                        Name = "Budget Single",
                        Description = "Cosy single room ideal for solo travellers and trekkers. Simple but clean with shared bathroom facilities and a warm atmosphere.",
                        PricePerNight = 25.00m,
                        MaxGuests = 1,
                        ImageFileName = "image2.jpg",
                        RoomType = "Budget",
                        IsAvailable = true
                    }
                };

                context.Properties.AddRange(properties); // Add all properties at once
                context.SaveChanges(); // Persist to database
            }

            // ---- SEED GALLERY IMAGES ----
            // Only add gallery images if the table is empty
            if (!context.GalleryImages.Any())
            {
                var images = new List<GalleryImage>
                {
                    new GalleryImage
                    {
                        Caption = "Guesthouse Night View",
                        AltText = "Hunza Haven Guest House illuminated at night",
                        FileName = "image1.jpg",
                        DisplayOrder = 1
                    },
                    new GalleryImage
                    {
                        Caption = "Guesthouse Exterior",
                        AltText = "Exterior view of Hunza Haven Guest House building",
                        FileName = "image2.jpg",
                        DisplayOrder = 2
                    },
                    new GalleryImage
                    {
                        Caption = "Dining Room",
                        AltText = "Traditional dining room inside the guesthouse",
                        FileName = "image3.jpg",
                        DisplayOrder = 3
                    },
                    new GalleryImage
                    {
                        Caption = "Sunrise Over the Valley",
                        AltText = "Sunrise view over Hunza Valley from the guesthouse",
                        FileName = "image4.jpg",
                        DisplayOrder = 4
                    },
                    new GalleryImage
                    {
                        Caption = "Standard Bedroom",
                        AltText = "Standard bedroom with mountain view at Hunza Haven",
                        FileName = "image5.jpg",
                        DisplayOrder = 5
                    },
                    new GalleryImage
                    {
                        Caption = "View from Guesthouse",
                        AltText = "Panoramic mountain view from the guesthouse terrace",
                        FileName = "image6.jpg",
                        DisplayOrder = 6
                    }
                };

                context.GalleryImages.AddRange(images); // Add all images at once
                context.SaveChanges(); // Persist to database
            }
        }

        /// <summary>
        /// Simple password hashing method using SHA256.
        /// In a production environment, BCrypt or Argon2 would be preferred.
        /// </summary>
        /// <param name="password">The plain-text password to hash</param>
        /// <returns>A Base64-encoded SHA256 hash of the password</returns>
        public static string HashPassword(string password)
        {
            // Code adapted from Microsoft, 2024
            // https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.sha256
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
            // End of adapted code
        }
    }
}
