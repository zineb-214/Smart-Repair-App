using AuthApp.Models;
using Microsoft.AspNetCore.Mvc;
using AuthApp.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Controllers
{
    public class PaiementController : Controller
    {
        private readonly Context _context;

        public PaiementController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Createee(int magasinId)
        {
            if (magasinId == 0)
            {
                return RedirectToAction("Create", "Magasin"); // or another fallback
            }

            // Store the MagasinId in TempData for later use (in case we need it after post)
            TempData["MagasinId"] = magasinId;

            // Fetch the corresponding Magasin (optional, just to display/store name)
            var magasin = _context.Magasins.Find(magasinId);
            if (magasin != null)
            {
                TempData["StoreName"] = magasin.Nom;
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Createee(Paiement paiement)
        {
            if (ModelState.IsValid)
            {
                // Get Magasin
                var magasin = await _context.Magasins.FirstOrDefaultAsync(m => m.Id == paiement.MagasinId);
                if (magasin == null)
                {
                    ModelState.AddModelError("", "Magasin introuvable.");
                    return View(paiement);
                }

                // Assign Magasin to Paiement
                paiement.Magasin = magasin;
                paiement.DatePaiement = DateTime.Now;

                // Save Paiement
                _context.Paiements.Add(paiement);
                await _context.SaveChangesAsync();

                // Store name for next step
                TempData["StoreName"] = magasin.Nom;
                TempData["MagasinId"] = magasin.Id;

                // Create Abonnement
                var abonnement = new Abonnement_magasin
                {
                    Id_magasin = magasin.Id,
                    Magasin = magasin,
                    Statut = "Actif"
                };

                _context.Abonnement_Magasins.Add(abonnement);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(paiement);
        }
    }
}
