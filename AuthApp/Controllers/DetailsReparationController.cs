using AuthApp.Data;
using AuthApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApp.Controllers
{
    public class DetailsReparationController : Controller
    {
        private readonly Context _context;

        public DetailsReparationController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var details = await _context.DetailsReparations
                                        .Include(d => d.Reparation)
                                        .Include(d => d.Appareil)
                                        .ToListAsync();
            return View(details);
        }

        public IActionResult Create(int reparationId)
        {
            var model = new DetailsReparation { ReparationId = reparationId };
            ViewBag.Appareils = _context.Appareils.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetailsReparation detailsReparation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detailsReparation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Reparation");
            }

            ViewBag.Appareils = _context.Appareils.ToList();
            return View(detailsReparation);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0) return NotFound();

            var detail = await _context.DetailsReparations.FindAsync(id);
            if (detail == null) return NotFound();

            ViewBag.Appareils = _context.Appareils.ToList(); // Load Appareils
            return View(detail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DetailsReparation detailsReparation)
        {
            if (id != detailsReparation.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detailsReparation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.DetailsReparations.Any(e => e.Id == detailsReparation.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("Index", "Reparation");
            }

            ViewBag.Appareils = _context.Appareils.ToList(); // Reload Appareils in case of validation failure
            return View(detailsReparation);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var detail = await _context.DetailsReparations
                .Include(d => d.Reparation)
                .Include(d => d.Appareil)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (detail == null)
            {
                return NotFound();
            }

            return View(detail);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detail = await _context.DetailsReparations.FindAsync(id);
            if (detail == null)
            {
                return NotFound();
            }

            try
            {
                _context.DetailsReparations.Remove(detail);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Repair detail deleted successfully";
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = "Could not delete repair detail. Please try again.";
            }

            return RedirectToAction("Index", "Reparation");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var detail = await _context.DetailsReparations
                .Include(d => d.Appareil)
                .Include(d => d.Reparation)
                    .ThenInclude(r => r.Client)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (detail == null)
            {
                return NotFound();
            }

            // Calculate payment status
            ViewBag.PaymentStatus = detail.PrixReste == 0 ? "Paid in Full" :
                                  (detail.PrixAvance == 0 ? "Unpaid" : "Partially Paid");

            return View(detail);
        }



    }
}
