using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AuthApp.Data;
using AuthApp.Models;
using AuthApp.Services;
using Microsoft.EntityFrameworkCore;
// Add this using statement at the top
using X.PagedList;

namespace AuthApp.Controllers
{
    public class RecetteReparateurController : BaseController
    {
        private readonly Context _context;

        public RecetteReparateurController(Context context, JwtService jwtService, ILogger<ReparationController> logger)
            : base(jwtService, logger)
        {
            _context = context;
        }



    public IActionResult Index(int? page)
    {
        var (userId, username, role) = GetCurrentUser();

        if (userId == null || role != "Reparateur")
        {
            _logger.LogWarning("Unauthorized access attempt to RecetteReparateur");
            return RedirectToLogin();
        }

        if (!int.TryParse(userId, out var reparateurId))
        {
            _logger.LogError("Invalid reparateur ID format in token");
            return RedirectToLogin();
        }

        // Generate daily recettes for the logged-in reparateur
        GenerateDailyRecettes(reparateurId);

        // Set up pagination
        int pageSize = 5; // Same as ClientController
        int pageNumber = page ?? 1;

        // Retrieve recettes with pagination
        var recettes = _context.RecetteReparateurs
                               .Where(r => r.ReparateurId == reparateurId)
                               .Include(r => r.Reparateur)
                               .OrderByDescending(r => r.Date)
                               .ToPagedList(pageNumber, pageSize);

        return View(recettes);
    }


    private void GenerateDailyRecettes(int reparateurId)
        {
            var today = DateTime.Today;

            // Check if there are already recettes for today for the logged-in reparateur
            var existingRecette = _context.RecetteReparateurs
                                          .FirstOrDefault(r => r.Date == today && r.ReparateurId == reparateurId);

            if (existingRecette == null)
            {
                // If no recette for today, generate a new one
                var reparateur = _context.Reparateurs
                                         .FirstOrDefault(r => r.reparateurID == reparateurId);

                if (reparateur != null)
                {
                    // Calculate total repairs and total loans for today
                    var totalReparations = _context.Reparations
                        .Where(r => r.IdReparateur == reparateurId && r.DateReception == today)
                        .Sum(r => r.DetailsReparations.Sum(d => d.PrixTotalReparation));

                    var totalEmprunts = _context.Emprunts
                        .Where(e => e.Collegue.ReparateurId == reparateurId && e.Date == today)
                        .Sum(e => e.Montant);

                    // Create new recette record
                    var newRecette = new RecetteReparateur
                    {
                        ReparateurId = reparateurId,
                        Date = today
                    };

                    // Call the method to calculate values
                    newRecette.CalculerValeurs(totalReparations, totalEmprunts, reparateur.TaxPer);

                    // Add the new recette to the database
                    _context.RecetteReparateurs.Add(newRecette);
                    _context.SaveChanges();
                }
            }
            else
            {
                // If recette for today already exists, update it
                var reparateur = _context.Reparateurs
                                         .FirstOrDefault(r => r.reparateurID == reparateurId);

                if (reparateur != null)
                {
                    // Calculate total repairs and total loans for today
                    var totalReparations = _context.Reparations
                        .Where(r => r.IdReparateur == reparateurId && r.DateReception == today)
                        .Sum(r => r.DetailsReparations.Sum(d => d.PrixTotalReparation));

                    var totalEmprunts = _context.Emprunts
                        .Where(e => e.Collegue.ReparateurId == reparateurId && e.Date == today)
                        .Sum(e => e.Montant);

                    // Update the existing recette record
                    existingRecette.CalculerValeurs(totalReparations, totalEmprunts, reparateur.TaxPer);

                    _context.RecetteReparateurs.Update(existingRecette);
                    _context.SaveChanges();
                }
            }
        }


    }
}
