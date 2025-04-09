using AuthApp.Data;
using AuthApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthApp.Controllers
{
    [Authorize(Roles = "Magasin")]
    public class RecetteMagasinController : Controller
    {
        private readonly Context _context;
        public RecetteMagasinController(Context context)
        {
            _context = context;
        }



      

            // GET: RecetteMagasin
            public async Task<IActionResult> Index()
            {
                // Récupérer toutes les recettes réparateurs associées au magasin actuel
                var magasinId = GetCurrentMagasinId();  // Suppose que tu as une méthode pour récupérer l'ID du magasin connecté
                var recettesReparateurs = await _context.RecetteReparateurs
                    .Where(rr => rr.Reparateur.MagasinId == magasinId)  // Récupérer les recettes associées à ce magasin
                    .Include(rr => rr.Reparateur)
                    .ToListAsync();

                // Calculer les totaux pour la recette du magasin sans associer les recettes réparateurs
                var recetteMagasin = new RecetteMagasin
                {
                    MagasinId = magasinId,
                    NomMagasin = (await _context.Magasins.FindAsync(magasinId))?.Nom,
                    Date = DateTime.Today,
                    RecetteTotaleMagasin = recettesReparateurs.Sum(rr => rr.RecetteTotale),  // Somme des recettes des réparateurs
                    TotalTax = recettesReparateurs.Sum(rr => rr.TaxReparateur)  // Somme des taxes des réparateurs
                };

                // Retourner la vue avec les recettes du magasin et les recettes des réparateurs
                ViewBag.RecetteMagasin = recetteMagasin;  // Optionnel: pour afficher les totaux dans la vue
                return View(recettesReparateurs);  // Afficher toutes les recettes des réparateurs liées au magasin
            }

            // POST: Générer la recette du magasin
            [HttpPost]
            public async Task<IActionResult> GenererRecette()
            {
                // Récupérer toutes les recettes réparateurs non associées
                var recettesReparateurs = await _context.RecetteReparateurs
                    .Where(rr => rr.RecetteMagasin == null)  // S'assurer que la recette n'a pas encore été associée à un magasin
                    .Include(rr => rr.Reparateur)
                    .ToListAsync();

                // Groupement des recettes réparateurs par magasin
                var grouped = recettesReparateurs
                    .GroupBy(rr => rr.Reparateur.Magasin)
                    .ToList();

                // Création des recettes magasin
                foreach (var group in grouped)
                {
                    var magasin = group.Key;
                    var recetteMagasin = new RecetteMagasin
                    {
                        NomMagasin = magasin.Nom,
                        MagasinId = magasin.Id,
                        Date = DateTime.Today,
                        RecetteTotaleMagasin = group.Sum(rr => rr.RecetteTotale),  // Somme des recettes des réparateurs
                        TotalTax = group.Sum(rr => rr.TaxReparateur)  // Somme des taxes des réparateurs
                    };

                    // Pas besoin d'associer directement les recettes réparateurs ici
                    // Les recettes réparateurs seront simplement prises en compte dans le calcul de la recette totale du magasin

                    _context.RecetteMagasins.Add(recetteMagasin);  // Ajouter la nouvelle recette magasin
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));  // Rediriger après avoir généré les recettes
            }

            // Méthode fictive pour récupérer l'ID du magasin connecté (à adapter selon ta logique d'authentification)
            private int GetCurrentMagasinId()
            {
                var currentUserId = User.Identity.Name;  // Exemple : récupérer le nom d'utilisateur
                var magasin = _context.Magasins.FirstOrDefault(m => m.UserName == currentUserId);  // Recherche du magasin par le nom d'utilisateur
                return magasin?.Id ?? 0;  // Retourner l'ID du magasin ou 0 si non trouvé
            }

        }
    }
