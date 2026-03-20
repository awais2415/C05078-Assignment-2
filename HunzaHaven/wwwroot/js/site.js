// wwwroot/js/site.js - Main JavaScript file for Hunza Haven Guest House
// Handles the responsive hamburger navigation menu and the gallery carousel

// ============================================================
// HAMBURGER MENU TOGGLE
// Opens and closes the mobile navigation menu when the button is clicked
// ============================================================

// Get references to the hamburger button and navigation links
var hamburger = document.getElementById("hamburger");
var navLinks = document.getElementById("navLinks");

// Add click event listener to toggle the menu open/closed
if (hamburger && navLinks) {
    hamburger.addEventListener("click", function () {
        // Toggle the 'open' CSS class which shows/hides the menu
        navLinks.classList.toggle("open");

        // Update the aria-label for accessibility
        var isOpen = navLinks.classList.contains("open");
        hamburger.setAttribute("aria-label", isOpen ? "Close navigation menu" : "Open navigation menu");
    });
}

// ============================================================
// GALLERY CAROUSEL
// Provides previous/next navigation and dot indicators for the image slideshow
// Code adapted from W3Schools, 2024
// https://www.w3schools.com/howto/howto_js_slideshow.asp
// ============================================================

// Get references to carousel elements
var slides = document.querySelectorAll(".carousel-slide");
var dots = document.querySelectorAll(".carousel-dot");
var prevBtn = document.getElementById("prevBtn");
var nextBtn = document.getElementById("nextBtn");

// Only initialise carousel if slides exist on the page
if (slides.length > 0) {

    // Track the currently displayed slide index
    var currentSlide = 0;

    /**
     * showSlide - Displays the slide at the given index
     * Hides all other slides and updates the active dot indicator
     * @param {number} index - The index of the slide to show
     */
    function showSlide(index) {
        // Wrap around if index goes out of bounds
        if (index >= slides.length) {
            currentSlide = 0;
        } else if (index < 0) {
            currentSlide = slides.length - 1;
        } else {
            currentSlide = index;
        }

        // Hide all slides by removing the 'active' class
        for (var i = 0; i < slides.length; i++) {
            slides[i].classList.remove("active");
        }

        // Remove active state from all dots
        for (var j = 0; j < dots.length; j++) {
            dots[j].classList.remove("active");
        }

        // Show the current slide and highlight its dot
        slides[currentSlide].classList.add("active");
        if (dots[currentSlide]) {
            dots[currentSlide].classList.add("active");
        }
    }

    // Add click event for the Previous button
    if (prevBtn) {
        prevBtn.addEventListener("click", function () {
            showSlide(currentSlide - 1);
        });
    }

    // Add click event for the Next button
    if (nextBtn) {
        nextBtn.addEventListener("click", function () {
            showSlide(currentSlide + 1);
        });
    }

    // Add click events for each dot indicator
    for (var d = 0; d < dots.length; d++) {
        dots[d].addEventListener("click", function () {
            // Get the slide index from the data-index attribute
            var slideIndex = parseInt(this.getAttribute("data-index"));
            showSlide(slideIndex);
        });
    }

    // Auto-advance the carousel every 5 seconds
    setInterval(function () {
        showSlide(currentSlide + 1);
    }, 5000);
}
// End of adapted code

// ============================================================
// FORM VALIDATION ENHANCEMENT
// Adds real-time visual feedback when required fields are completed
// ============================================================

// Get all form inputs with validation
var validatedInputs = document.querySelectorAll("input[required], textarea[required]");

// Add input event listeners for real-time feedback
for (var v = 0; v < validatedInputs.length; v++) {
    validatedInputs[v].addEventListener("input", function () {
        // Find the associated error span
        var errorSpan = this.parentElement.querySelector(".field-error");
        // Clear the error message when the user starts typing
        if (errorSpan && this.value.trim() !== "") {
            errorSpan.textContent = "";
        }
    });
}

// ============================================================
// SCROLL TO TOP FUNCTIONALITY
// Smooth scroll to top when clicking the footer brand
// ============================================================

var footerBrand = document.querySelector(".footer-brand");
if (footerBrand) {
    footerBrand.style.cursor = "pointer";
    footerBrand.addEventListener("click", function () {
        window.scrollTo({ top: 0, behavior: "smooth" });
    });
}
