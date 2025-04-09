using AuthApp.Data;
using AuthApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Controllers
{
    [Authorize(Roles = "Reparateur")]
    public class ReparateurPageController : BaseController
    {
        private readonly Context _context;

        public ReparateurPageController(Context context, JwtService jwtService, ILogger<ReparateurPageController> logger)
            : base(jwtService, logger)
        {
            _context = context;
        }


        public async Task<IActionResult> PageReparateur()
        {
            var userId = User.Identity.Name;
            var reparateur = await _context.Reparateurs.FirstOrDefaultAsync(u => u.UserName == userId);

            if (reparateur == null)
            {
                return NotFound("Réparateur non trouvé.");
            }

            // Récupérer toutes les réparations du réparateur avec leurs détails et statuts
            var reparationsDetails = await _context.DetailsReparations
                .Include(d => d.Reparation)  // Jointure avec la table Reparation
                .Where(d => d.Reparation.IdReparateur == reparateur.reparateurID)
                .ToListAsync();

            // Calcul du nombre de réparations par statut
            int nombreReparations = reparationsDetails.Count();
            int reparationsEnAttente = reparationsDetails.Count(d => d.Statut == "En Attente");
            int reparationsTerminees = reparationsDetails.Count(d => d.Statut == "Terminé");
            int reparationsEnCour = reparationsDetails.Count(d => d.Statut == "En Cours");

            // Passer les données à la vue via ViewBag
            ViewBag.NombreReparations = nombreReparations;
            ViewBag.ReparationsEnCours = reparationsEnCour;
            ViewBag.ReparationsTerminees = reparationsTerminees;
            ViewBag.ReparationsEnAttente = reparationsEnAttente;

            return View();
        }



    }
}
