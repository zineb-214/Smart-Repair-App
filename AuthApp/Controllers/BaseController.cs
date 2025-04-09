using AuthApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace AuthApp.Controllers
{
    public class BaseController : Controller
    {
        protected readonly JwtService _jwtService;
        protected readonly ILogger<BaseController> _logger;

        public BaseController(JwtService jwtService, ILogger<BaseController> logger)
        {
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected (string userId, string username, string role) GetCurrentUser()
        {
            try
            {
                var token = Request.Cookies["JWTToken"];
                if (string.IsNullOrEmpty(token))
                {
                    _logger.LogWarning("JWT token not found in cookies");
                    return (null, null, null);
                }

                var principal = _jwtService.ValidateToken(token);
                if (principal == null)
                {
                    _logger.LogWarning("Invalid JWT token");
                    return (null, null, null);
                }

                return (
                    principal.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    principal.FindFirst(ClaimTypes.Name)?.Value,
                    principal.FindFirst(ClaimTypes.Role)?.Value
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting current user");
                return (null, null, null);
            }
        }

        protected IActionResult RedirectToLogin()
        {
            Response.Cookies.Delete("JWTToken");
            return RedirectToAction("Login", "Account");
        }

        protected bool IsUserInRole(string role)
        {
            var (_, _, userRole) = GetCurrentUser();
            return string.Equals(userRole, role, StringComparison.OrdinalIgnoreCase);
        }

        protected string GetCurrentUserId()
        {
            var (userId, _, _) = GetCurrentUser();
            return userId;
        }

        protected IActionResult HandleUnauthorized()
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        protected void LogUserAction(string action)
        {
            var (userId, username, role) = GetCurrentUser();
            _logger.LogInformation($"User {username} (ID: {userId}, Role: {role}) performed: {action}");
        }
    }
}