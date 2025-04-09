using Microsoft.AspNetCore.Mvc;
using AuthApp.Models;
using AuthApp.Data;
using Microsoft.EntityFrameworkCore;
using AuthApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace AuthApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly Context _context;
        private readonly JwtService _jwtService;

        public AccountController(Context context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Vérifier Admin
            var admin = await _context.Admins
                .FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

            if (admin != null)
            {
                var token = _jwtService.GenerateToken(admin.Id.ToString(), admin.Username, "Admin");
                SetJwtCookie(token);
                return RedirectToAction("Index", "Admin");
            }

            // Vérifier Magasin
            var magasin = await _context.Magasins
                .FirstOrDefaultAsync(m => m.UserName == model.Username && m.Password == model.Password);

            if (magasin != null)
            {
                var token = _jwtService.GenerateToken(magasin.Id.ToString(), magasin.UserName, "Magasin");
                SetJwtCookie(token);
                return RedirectToAction("PageMagasin", "MagasinPage");
            }

            // Vérifier Réparateur
            var reparateur = await _context.Reparateurs
                .FirstOrDefaultAsync(r => r.UserName == model.Username && r.Password == model.Password);

            if (reparateur != null)
            {
                var token = _jwtService.GenerateToken(reparateur.reparateurID.ToString(), reparateur.UserName, "Reparateur");
                SetJwtCookie(token);
                //  return RedirectToAction("PageReparateur", "ReparateurPage");
                return RedirectToAction("PageReparateur", "ReparateurPage");
            }

            ModelState.AddModelError("", "Identifiant ou mot de passe invalide");
            return View(model);
        }

        private void SetJwtCookie(string token)
        {
            Response.Cookies.Append("JWTToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.Now.AddMinutes(_jwtService.TokenExpiryMinutes)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("JWTToken");
            return RedirectToAction("Index", "Home");
        }
    }
}