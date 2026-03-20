// Program.cs - Main entry point for Hunza Haven Guest House web application
// Configures services including Entity Framework, authentication and Razor Pages

using HunzaHaven.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

// Create the web application builder
var builder = WebApplication.CreateBuilder(args);

// ---- SERVICE REGISTRATION ----

// Register Razor Pages service for server-side page rendering
builder.Services.AddRazorPages();

// Register Entity Framework Core with SQL Server provider
// Connection string is read from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure cookie-based authentication for admin login
// Code adapted from Microsoft, 2024
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";          // Redirect here if not authenticated
        options.LogoutPath = "/Logout";        // Path for sign-out
        options.AccessDeniedPath = "/Login";   // Redirect if access is denied
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Session timeout
    });
// End of adapted code

// Register authorization services
builder.Services.AddAuthorization();

// Build the application from the configured services
var app = builder.Build();

// ---- SEED THE DATABASE ----
// Ensure database is created and seed initial data on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated(); // Creates database if it does not exist
    DbInitialiser.Seed(context);      // Seeds initial property and gallery data
}

// ---- MIDDLEWARE PIPELINE ----

app.UsePathBase("/CO5078");

// Show detailed errors only in development environment
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // Enforce HTTPS in production
}

app.UseHttpsRedirection();  // Redirect HTTP requests to HTTPS
app.UseStaticFiles();       // Serve files from wwwroot (CSS, JS, images)
app.UseRouting();           // Enable endpoint routing

app.UseAuthentication();    // Enable authentication middleware
app.UseAuthorization();     // Enable authorization middleware

app.MapRazorPages();        // Map Razor Pages endpoints

// Start the application
app.Run();