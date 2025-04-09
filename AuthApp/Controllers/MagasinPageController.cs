using AuthApp.Data;
using AuthApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthApp.Controllers
{
    [Authorize(Roles = "Magasin")]
    public class MagasinPageController : Controller
    {
        private readonly Context _context;

        public IActionResult Index()
        {
            var recetteMagasins = _context.RecetteMagasins.ToList();  // Récupérer toutes les recettes du magasin
            return View(recetteMagasins);
            // return RedirectToAction("PageMagasin");
        }



        public MagasinPageController(Context context)
        {
            _context = context;
        }

        private int GetCurrentMagasinId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }








        public async Task<IActionResult> PageMagasin()
        {
            var magasinId = GetCurrentMagasinId();

            // ✅ Génération automatique de la recette si besoin
            var recettesReparateurs = await _context.RecetteReparateurs
                .Where(rr => rr.RecetteMagasin == null && rr.Reparateur.MagasinId == magasinId)
                .Include(rr => rr.Reparateur)
                .ToListAsync();

            if (recettesReparateurs.Any())
            {
                var magasin = await _context.Magasins.FindAsync(magasinId);

                var recetteMagasin = new RecetteMagasin
                {
                    NomMagasin = magasin.Nom,
                    MagasinId = magasin.Id,
                    Date = DateTime.Today,
                    RecetteTotaleMagasin = recettesReparateurs.Sum(rr => rr.RecetteTotale),  // Somme des recettes
                    TotalTax = recettesReparateurs.Sum(rr => rr.TaxReparateur)  // Somme des taxes
                };

                // Pas besoin d'associer directement les recettes réparateurs ici.
                // Nous ne mettons pas à jour le RecetteMagasinId des RecetteReparateurs

                _context.RecetteMagasins.Add(recetteMagasin);  // Ajouter la nouvelle recette magasin
                await _context.SaveChangesAsync();
            }

            // Récupération des données
            var recettes = await GetRecettesMagasin(magasinId);
            var reparateurs = await _context.Reparateurs
                .Where(r => r.MagasinId == magasinId)
                .ToListAsync();
            var magasinNom = (await _context.Magasins.FindAsync(magasinId))?.Nom;

            // Passage des données
            ViewBag.Reparateurs = reparateurs;
            ViewBag.MagasinNom = magasinNom;

            return View(recettes);
        }









        private async Task<List<RecetteMagasin>> GetRecettesMagasin(int magasinId)
        {
            return await _context.RecetteMagasins
                .Where(rm => rm.MagasinId == magasinId)
                .Include(rm => rm.RecettesReparateurs)
                    .ThenInclude(rr => rr.Reparateur)
                .OrderByDescending(rm => rm.Date)
                .Take(5)
                .ToListAsync() ?? new List<RecetteMagasin>(); // Retourne une liste vide si null
        }




        public async Task<IActionResult> RecetteMagasinPartial()
        {
            var magasinId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var recettes = await _context.RecetteMagasins
                .Where(rm => rm.MagasinId == magasinId)
                .Include(rm => rm.RecettesReparateurs)
                    .ThenInclude(rr => rr.Reparateur)
                .OrderByDescending(rm => rm.Date)
                .Take(5) // Limiter à 5 dernières recettes
                .ToListAsync();

            return PartialView("RecetteMagasinPartial", recettes);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var magasinId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var recetteMagasin = await _context.RecetteMagasins
                .Include(rm => rm.RecettesReparateurs)  // Inclure les liens avec RecetteReparateur
                .ThenInclude(rr => rr.Reparateur)  // Inclure le réparateur lié à chaque RecetteReparateur
                .FirstOrDefaultAsync(rm => rm.Id == id && rm.MagasinId == magasinId);

            if (recetteMagasin == null)
            {
                return NotFound();
            }

            return View("~/Views/MagasinPage/DetailRecette.cshtml", recetteMagasin);
        }


        // GET: Recette/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Récupère la recette avec les réparateurs associés
            var recetteMagasin = await _context.RecetteMagasins
                .Include(r => r.RecettesReparateurs)
                .ThenInclude(rr => rr.Reparateur) // Inclut les réparateurs associés
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recetteMagasin == null)
            {
                return NotFound();
            }

            return View(recetteMagasin); // Passe la recette à la vue pour confirmation
        }

        // POST: Recette/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recetteMagasin = await _context.RecetteMagasins
                .Include(r => r.RecettesReparateurs)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recetteMagasin != null)
            {
                _context.RecetteMagasins.Remove(recetteMagasin);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index)); // Redirige vers la page d'index après suppression
        }




    }
}
