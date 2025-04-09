using AuthApp.Data;
using AuthApp.Models;
using AuthApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Mvc.Core;

namespace AuthApp.Controllers
{
    public class ReparationController : BaseController
    {
        private readonly Context _context;

        public ReparationController(Context context, JwtService jwtService, ILogger<ReparationController> logger)
            : base(jwtService, logger)
        {
            _context = context;
        }

 

public async Task<IActionResult> Index(int? reparationsPage, int? detailsPage, int? selectedReparationId)
    {
        var (userId, username, role) = GetCurrentUser();

        if (userId == null || role != "Reparateur")
        {
            _logger.LogWarning("Unauthorized access attempt to Reparations");
            return RedirectToLogin();
        }

        if (!int.TryParse(userId, out var reparateurId))
        {
            _logger.LogError("Invalid reparateur ID format in token");
            return RedirectToLogin();
        }

        int pageSize = 5; // Number of items per page
        int reparationsPageNumber = reparationsPage ?? 1;
        int detailsPageNumber = detailsPage ?? 1;

        var reparations = await _context.Reparations
            .Where(r => r.IdReparateur == reparateurId)
            .Include(r => r.Client)
            .Include(r => r.Reparateur)
            .OrderByDescending(r => r.DateReception)
            .ToPagedListAsync(reparationsPageNumber, pageSize);

            IPagedList<DetailsReparation> detailsList = new List<DetailsReparation>().ToPagedList(1, pageSize);


            if (selectedReparationId.HasValue)
        {
            detailsList = await _context.DetailsReparations
                .Where(d => d.ReparationId == selectedReparationId)
                .Include(d => d.Appareil)
                .ToPagedListAsync(detailsPageNumber, pageSize);
        }

        ViewBag.SelectedReparationId = selectedReparationId;
        ViewBag.DetailsPage = detailsPageNumber;

        return View(new Tuple<IPagedList<Reparation>, IPagedList<DetailsReparation>>(reparations, detailsList));
    }


    public async Task<IActionResult> GetDetails(int id)
        {
            var (userId, _, role) = GetCurrentUser();
            if (userId == null || role != "Reparateur")
            {
                return Json(new { error = "Unauthorized" });
            }

            var details = await _context.DetailsReparations
                .Where(d => d.ReparationId == id)
                .Include(d => d.Appareil)
                .ToListAsync();

            return Json(details);
        }

        public IActionResult CreateDetails(int reparationId)
        {
            var (userId, _, role) = GetCurrentUser();
            if (userId == null || role != "Reparateur")
            {
                return RedirectToLogin();
            }

            ViewBag.ReparationId = reparationId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDetails(int reparationId, DetailsReparation detailsReparation)
        {
            var (userId, _, role) = GetCurrentUser();
            if (userId == null || role != "Reparateur")
            {
                return RedirectToLogin();
            }

            if (ModelState.IsValid)
            {
                detailsReparation.ReparationId = reparationId;
                _context.Add(detailsReparation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            var (userId, _, role) = GetCurrentUser();
            if (userId == null || role != "Reparateur")
            {
                return RedirectToLogin();
            }

            if (!int.TryParse(userId, out var reparateurId))
            {
                return RedirectToLogin();
            }

            // Get only clients assigned to this reparateur
            var clients = _context.Clients
                .Where(c => c.ReparateurId == reparateurId)
                .ToList();

            ViewData["Clients"] = new SelectList(clients, "Id", "Nom");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reparation reparation)
        {
            var (userId, _, role) = GetCurrentUser();
            if (userId == null || role != "Reparateur" || !int.TryParse(userId, out var reparateurId))
            {
                return RedirectToLogin();
            }

            if (ModelState.IsValid)
            {
                reparation.IdReparateur = reparateurId; // Set to logged-in reparateur
                _context.Add(reparation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Clients"] = new SelectList(_context.Clients, "Id", "Nom", reparation.IdClient);
            return View(reparation);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var (userId, _, role) = GetCurrentUser();
            if (userId == null || role != "Reparateur")
            {
                return RedirectToLogin();
            }

            if (id == null)
            {
                return NotFound();
            }

            var reparation = await _context.Reparations.FindAsync(id);
            if (reparation == null)
            {
                return NotFound();
            }

            // Verify the reparation belongs to the current reparateur
            if (!int.TryParse(userId, out var reparateurId) || reparation.IdReparateur != reparateurId)
            {
                return RedirectToLogin();
            }

            // Get only clients assigned to this reparateur
            var clients = _context.Clients
                .Where(c => c.ReparateurId == reparateurId)
                .ToList();

            ViewData["Clients"] = new SelectList(clients, "Id", "Nom", reparation.IdClient);
            return View(reparation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reparation reparation)
        {
            var (userId, _, role) = GetCurrentUser();
            if (userId == null || role != "Reparateur" || !int.TryParse(userId, out var reparateurId))
            {
                return RedirectToLogin();
            }

            if (id != reparation.Id)
            {
                return NotFound();
            }

            // Verify the original reparation belongs to the current reparateur
            var existingReparation = await _context.Reparations.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (existingReparation == null || existingReparation.IdReparateur != reparateurId)
            {
                return RedirectToLogin();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure the reparateur can't be changed
                    reparation.IdReparateur = reparateurId;
                    _context.Update(reparation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReparationExists(reparation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Get only clients assigned to this reparateur
            var clients = _context.Clients
                .Where(c => c.ReparateurId == reparateurId)
                .ToList();

            ViewData["Clients"] = new SelectList(clients, "Id", "Nom", reparation.IdClient);
            return View(reparation);
        }

        private bool ReparationExists(int id)
        {
            return _context.Reparations.Any(e => e.Id == id);
        }

        // GET: Reparation/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var (userId, _, role) = GetCurrentUser();
            if (userId == null || role != "Reparateur")
            {
                _logger.LogWarning("Unauthorized delete attempt");
                return RedirectToLogin();
            }

            if (id == null)
            {
                return NotFound();
            }

            if (!int.TryParse(userId, out var reparateurId))
            {
                _logger.LogError("Invalid reparateur ID format");
                return RedirectToLogin();
            }

            var reparation = await _context.Reparations
                .Include(r => r.Client)
                .Include(r => r.Reparateur)
                .Include(r => r.DetailsReparations)
                .FirstOrDefaultAsync(r => r.Id == id && r.IdReparateur == reparateurId);

            if (reparation == null)
            {
                return NotFound();
            }

            return View(reparation);
        }

        // POST: Reparation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (userId, _, role) = GetCurrentUser();
            if (userId == null || role != "Reparateur")
            {
                _logger.LogWarning("Unauthorized delete attempt");
                return RedirectToLogin();
            }

            if (!int.TryParse(userId, out var reparateurId))
            {
                _logger.LogError("Invalid reparateur ID format");
                return RedirectToLogin();
            }

            var reparation = await _context.Reparations
                .Include(r => r.DetailsReparations) // Include details for cascade delete
                .FirstOrDefaultAsync(r => r.Id == id && r.IdReparateur == reparateurId);

            if (reparation == null)
            {
                return NotFound();
            }

            try
            {
                // First remove all details
                _context.DetailsReparations.RemoveRange(reparation.DetailsReparations);

                // Then remove the reparation
                _context.Reparations.Remove(reparation);

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Reparation deleted successfully";
                _logger.LogInformation($"Reparation {id} deleted by reparateur {reparateurId}");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Error deleting reparation {id}");
                TempData["ErrorMessage"] = "Could not delete reparation. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }


        // ... keep the rest of your actions (Edit, Delete, etc.) with similar authorization checks
    }
}