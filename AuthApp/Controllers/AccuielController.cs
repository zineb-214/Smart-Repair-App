using AuthApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthApp.Controllers
{
    public class AccuielController : BaseController
    {
        private readonly JwtService _jwtService;
        private readonly ILogger<AccuielController> _logger;

        public AccuielController(JwtService jwtService, ILogger<AccuielController> logger)
            : base(jwtService, logger)
        {
            _jwtService = jwtService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Get current user from JWT (checking reparateur role)
            var (userId, username, role) = GetCurrentUser();

            if (role != "Reparateur" || string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Unauthorized access attempt by user: {UserId}", userId);
                return RedirectToLogin();
            }

            // Log the successful login and show the reparateur's name
            _logger.LogInformation("Reparateur {Username} logged in successfully", username);

            // Return view with reparateur's name and dashboard links
            ViewData["ReparateurName"] = username; // Assuming username is the reparateur's name
            return View();
        }

        // Add more actions similar to the ReparationController for navigation to different views (Client, Reparation, Collègue, RecetteReparateur)
        public IActionResult ClientDashboard()
        {
            var (userId, _, role) = GetCurrentUser();
            if (role != "Reparateur" || string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Unauthorized access attempt by user: {UserId}", userId);
                return RedirectToLogin();
            }

            // Logic for Client Dashboard
            return View();
        }

        public IActionResult ReparationDashboard()
        {
            var (userId, _, role) = GetCurrentUser();
            if (role != "Reparateur" || string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Unauthorized access attempt by user: {UserId}", userId);
                return RedirectToLogin();
            }

            // Logic for Reparation Dashboard
            return View();
        }

        public IActionResult ColleagueDashboard()
        {
            var (userId, _, role) = GetCurrentUser();
            if (role != "Reparateur" || string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Unauthorized access attempt by user: {UserId}", userId);
                return RedirectToLogin();
            }

            // Logic for Colleague Dashboard
            return View();
        }

        public IActionResult RecetteReparateurDashboard()
        {
            var (userId, _, role) = GetCurrentUser();
            if (role != "Reparateur" || string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Unauthorized access attempt by user: {UserId}", userId);
                return RedirectToLogin();
            }

            // Logic for Recette Reparateur Dashboard
            return View();
        }
    }
}
