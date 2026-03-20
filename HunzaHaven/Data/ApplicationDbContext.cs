// Data/ApplicationDbContext.cs - Entity Framework Core database context
// Defines the database schema through DbSet properties and configures the model

using HunzaHaven.Models;
using Microsoft.EntityFrameworkCore;

namespace HunzaHaven.Data
{
    /// <summary>
    /// ApplicationDbContext is the main database context class that manages
    /// the connection to SQL Server and provides access to database tables.
    /// Code adapted from Microsoft, 2024
    /// https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        // Constructor accepting options configured in Program.cs
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet for the Properties table - stores room/accommodation listings
        public DbSet<Property> Properties { get; set; } = null!;

        // DbSet for the GalleryImages table - stores gallery photo entries
        public DbSet<GalleryImage> GalleryImages { get; set; } = null!;

        // DbSet for the AdminUsers table - stores admin login credentials
        public DbSet<AdminUser> AdminUsers { get; set; } = null!;

        /// <summary>
        /// Configures the entity model using Fluent API.
        /// Sets up table names, column types and seed data constraints.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Property entity - set decimal precision for price column
            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("Properties");
                entity.Property(p => p.PricePerNight)
                      .HasColumnType("decimal(10,2)");
            });

            // Configure GalleryImage entity
            modelBuilder.Entity<GalleryImage>(entity =>
            {
                entity.ToTable("GalleryImages");
            });

            // Configure AdminUser entity - email must be unique
            modelBuilder.Entity<AdminUser>(entity =>
            {
                entity.ToTable("AdminUsers");
                entity.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
    // End of adapted code
}
