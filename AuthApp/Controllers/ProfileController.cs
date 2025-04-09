using AuthApp.Data;
using AuthApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Controllers
{
    [Authorize]
    [Route("Profile")]
    public class ProfileController : Controller
    {
        private readonly Context _context;

        public ProfileController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name;

            // Check all user types
            var admin = await _context.Admins.FirstOrDefaultAsync(u => u.Username == username);
            if (admin != null) return View("Profile", new ProfileViewModel
            {
                Id = admin.Id,
                Username = admin.Username,
                Password = admin.Password,
                UserType = "Admin"
            });

            var reparateur = await _context.Reparateurs.FirstOrDefaultAsync(u => u.UserName == username);
            if (reparateur != null) return View("Profile", new ProfileViewModel
            {
                Id = reparateur.reparateurID,
                Username = reparateur.UserName,
                Password = reparateur.Password,
                UserType = "Reparateur"
            });

            var magasin = await _context.Magasins.FirstOrDefaultAsync(u => u.UserName == username);
            if (magasin != null) return View("Profile", new ProfileViewModel
            {
                Id = magasin.Id,
                Username = magasin.UserName,
                Password = magasin.Password,
                UserType = "Magasin"
            });

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var username = User.Identity.Name;

            var admin = await _context.Admins.FirstOrDefaultAsync(u => u.Username == username);
            if (admin != null) return View("EditProfile", new ProfileViewModel
            {
                Id = admin.Id,
                Username = admin.Username,
                Password = admin.Password,
                UserType = "Admin"
            });

            var reparateur = await _context.Reparateurs.FirstOrDefaultAsync(u => u.UserName == username);
            if (reparateur != null) return View("EditProfile", new ProfileViewModel
            {
                Id = reparateur.reparateurID,
                Username = reparateur.UserName,
                Password = reparateur.Password,
                UserType = "Reparateur"
            });

            var magasin = await _context.Magasins.FirstOrDefaultAsync(u => u.UserName == username);
            if (magasin != null) return View("EditProfile", new ProfileViewModel
            {
                Id = magasin.Id,
                Username = magasin.UserName,
                Password = magasin.Password,
                UserType = "Magasin"
            });

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                string redirectAction = "Index"; // Default to admin

                string Controller = "Admin";

                switch (model.UserType)
                {
                    case "Admin":
                        var admin = await _context.Admins.FindAsync(model.Id);
                        if (admin != null)
                        {
                            admin.Username = model.Username;
                            admin.Password = model.Password;
                            _context.Update(admin);
                            redirectAction = "Index";
                            Controller = "Admin";
                        }
                        break;

                    case "Reparateur":
                        var reparateur = await _context.Reparateurs.FindAsync(model.Id);
                        if (reparateur != null)
                        {
                            reparateur.UserName = model.Username;
                            reparateur.Password = model.Password;
                            _context.Update(reparateur);
                            redirectAction = "PageReparateur";
                          Controller = "ReparateurPage";
                        }
                        break;

                    case "Magasin":
                        var magasin = await _context.Magasins.FindAsync(model.Id);
                        if (magasin != null)
                        {
                            magasin.UserName = model.Username;
                            magasin.Password = model.Password;
                            _context.Update(magasin);
                            redirectAction = "PageMagasin";
                            Controller = "MagasinPage";
                        }
                        break;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(redirectAction, Controller);
            }

            return View("EditProfile", model);
        }
    }
}
