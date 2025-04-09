using AuthApp.Data;
using AuthApp.Models;
using AuthApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthApp.Controllers
{
    [Authorize(Roles = "Reparateur")]
    public class CollegueController : BaseController
    {
        private readonly Context _context;
        private readonly ILogger<CollegueController> _logger;

        public CollegueController(
            Context context,
            JwtService jwtService,
            ILogger<CollegueController> logger)
            : base(jwtService, logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Collegue
        [Authorize(Roles = "Reparateur")]
        public async Task<IActionResult> Index()
        {
            var (userId, _, _) = GetCurrentUser();
            if (!int.TryParse(userId, out int reparateurId))
            {
                _logger.LogError("Invalid reparateur ID format");
                return RedirectToLogin();
            }

            var collegues = await _context.Collegues
                .Where(c => c.ReparateurId == reparateurId) // Only current reparateur's collegues
                .Include(c => c.Reparateur)
                .Include(c => c.Emprunts)
                .OrderBy(c => c.Nom)
                .ToListAsync();

            // Calculate total emprunts for display
            foreach (var collegue in collegues)
            {
                ViewData[$"TotalEmprunt_{collegue.Id}"] = collegue.Emprunts?.Sum(e => e.Montant) ?? 0;
            }

            return View(collegues);
        }

        [HttpGet]
        public async Task<JsonResult> GetEmprunts(int collegueId)
        {
            var emprunts = await _context.Emprunts
                .Where(e => e.CollegueId == collegueId)
                .Select(e => new {
                    Id = e.Id,  // Add this line
                    TypeEmprunt = e.TypeEmprunt.ToString(),
                    e.Montant,
                    e.Date
                }).ToListAsync();

            return Json(emprunts);
        }

        // GET: Collegue/Create
        [Authorize(Roles = "Reparateur")]
        public IActionResult Create()
        {
            var (userId, _, _) = GetCurrentUser();
            if (!int.TryParse(userId, out int reparateurId))
            {
                return RedirectToLogin();
            }

            // Automatically assign to current reparateur
            ViewBag.ReparateurId = reparateurId;
            ViewBag.ReparateurName = _context.Reparateurs
                .FirstOrDefault(r => r.reparateurID == reparateurId)?.Nom ?? "Your Account";

            return View();
        }

        // POST: Collegue/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Reparateur")]
        public async Task<IActionResult> Create([Bind("Nom,NumeroTelephone")] Collegue collegue)
        {
            var (userId, _, _) = GetCurrentUser();
            if (!int.TryParse(userId, out int reparateurId))
            {
                return RedirectToLogin();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Set reparateur and create
                    collegue.ReparateurId = reparateurId;
                    _context.Add(collegue);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating collegue");
                    ModelState.AddModelError("", "An error occurred while saving.");
                }
            }

            // Repopulate viewbag if validation fails
            ViewBag.ReparateurId = reparateurId;
            ViewBag.ReparateurName = _context.Reparateurs
                .FirstOrDefault(r => r.reparateurID == reparateurId)?.Nom ?? "Your Account";

            return View(collegue);
        }



        [Authorize(Roles = "Reparateur")]
        public async Task<IActionResult> Edit(int? id)
        {
            var (userId, _, _) = GetCurrentUser();
            if (!int.TryParse(userId, out int reparateurId))
            {
                _logger.LogError("Invalid reparateur ID format");
                return RedirectToLogin();
            }

            if (id == null)
            {
                return NotFound();
            }

            var collegue = await _context.Collegues
                .FirstOrDefaultAsync(c => c.Id == id && c.ReparateurId == reparateurId);

            if (collegue == null)
            {
                return NotFound();
            }

            // Show reparateur info for context
            ViewBag.ReparateurName = _context.Reparateurs
                .FirstOrDefault(r => r.reparateurID == reparateurId)?.Nom ?? "Your Account";

            return View(collegue);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Reparateur")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,NumeroTelephone")] Collegue collegue)
        {
            var (userId, _, _) = GetCurrentUser();
            if (!int.TryParse(userId, out int reparateurId))
            {
                return RedirectToLogin();
            }

            if (id != collegue.Id)
            {
                return NotFound();
            }

            // Verify the collegue belongs to this reparateur
            var existingCollegue = await _context.Collegues
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id && c.ReparateurId == reparateurId);

            if (existingCollegue == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Maintain the original reparateur association
                    collegue.ReparateurId = reparateurId;

                    _context.Update(collegue);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Colleague updated successfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Error updating colleague");
                   
                    throw;
                }
            }

            // Repopulate viewbag if validation fails
            ViewBag.ReparateurName = _context.Reparateurs
                .FirstOrDefault(r => r.reparateurID == reparateurId)?.Nom ?? "Your Account";

            return View(collegue);
        }


        [Authorize(Roles = "Reparateur")]
        public async Task<IActionResult> Delete(int? id)
        {
            var (userId, _, _) = GetCurrentUser();
            if (!int.TryParse(userId, out int reparateurId))
            {
                _logger.LogError("Invalid reparateur ID format");
                return RedirectToLogin();
            }

            if (id == null)
            {
                return NotFound();
            }

            var collegue = await _context.Collegues
                .Include(c => c.Reparateur)
                .Include(c => c.Emprunts)
                .FirstOrDefaultAsync(c => c.Id == id && c.ReparateurId == reparateurId);

            if (collegue == null)
            {
                return NotFound();
            }

            return View(collegue);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Reparateur")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (userId, _, _) = GetCurrentUser();
            if (!int.TryParse(userId, out int reparateurId))
            {
                return RedirectToLogin();
            }

            var collegue = await _context.Collegues
                .Include(c => c.Emprunts)
                .FirstOrDefaultAsync(c => c.Id == id && c.ReparateurId == reparateurId);

            if (collegue == null)
            {
                return NotFound();
            }

            try
            {
                // First delete all related emprunts
                _context.Emprunts.RemoveRange(collegue.Emprunts);

                // Then delete the collegue
                _context.Collegues.Remove(collegue);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Colleague {collegue.Nom} deleted successfully";
                _logger.LogInformation($"Colleague {id} deleted by reparateur {reparateurId}");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Error deleting colleague {id}");
                TempData["ErrorMessage"] = $"Could not delete colleague {collegue.Nom}. They may have active records.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CollegueExists(int id)
        {
            return _context.Collegues.Any(e => e.Id == id);
        }
    }
}