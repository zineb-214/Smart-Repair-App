using AuthApp.Data;
using AuthApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthApp.Controllers
{
    [Authorize(Roles = "Reparateur")]
    public class EmpruntController : Controller
    {
        private readonly Context _context;

        public EmpruntController(Context context)
        {
            _context = context;
        }

        [Authorize(Roles = "Reparateur")]
        public async Task<IActionResult> Index()
        {
            try
            {
                // Get the logged-in reparateur's ID from claims
                var reparateurId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var collegues = await _context.Collegues
                    .Where(c => c.ReparateurId == reparateurId) // Filter by logged-in reparateur
                    .Include(c => c.Reparateur) // Include reparateur details
                    .Include(c => c.Emprunts)   // Include emprunts
                    .OrderBy(c => c.Nom)        // Sort alphabetically
                    .ToListAsync();

                // Calculate total emprunts for each collegue
                foreach (var collegue in collegues)
                {
                    ViewData[$"TotalEmprunt_{collegue.Id}"] = collegue.Emprunts?.Sum(e => e.Montant) ?? 0;
                }

                return View(collegues);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error loading collegues index");
                TempData["ErrorMessage"] = "An error occurred while loading data.";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Emprunt/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprunt = await _context.Emprunts
                .Include(e => e.Collegue)
                .ThenInclude(c => c.Reparateur)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (emprunt == null)
            {
                return NotFound();
            }

            return View(emprunt);
        }

        // GET: Emprunt/Create
        public IActionResult Create(int? collegueId)
        {
            ViewBag.Collegues = new SelectList(_context.Collegues, "Id", "Nom");
            ViewBag.TypesEmprunt = Enum.GetValues(typeof(TypeEmprunt)).Cast<TypeEmprunt>().ToList();

            var emprunt = new Emprunt();

            if (collegueId.HasValue)
            {
                emprunt.CollegueId = collegueId.Value; // Pre-fill CollegueId
            }

            return View(emprunt);
        }


        // POST: Emprunt/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CollegueId,TypeEmprunt,Montant,Date")] Emprunt emprunt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emprunt);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Collegue");

            }
            ViewBag.Collegues = _context.Collegues.Include(c => c.Reparateur).ToList();
            ViewBag.TypesEmprunt = Enum.GetValues(typeof(TypeEmprunt)).Cast<TypeEmprunt>().ToList();
            return View(emprunt);
        }



        // GET: Emprunt/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprunt = await _context.Emprunts
                .Include(e => e.Collegue)
                .ThenInclude(c => c.Reparateur)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (emprunt == null)
            {
                return NotFound();
            }

            // Verify the emprunt belongs to a collegue of the logged-in reparateur
            var reparateurId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (emprunt.Collegue.ReparateurId != reparateurId)
            {
                return Forbid(); // Or return a not found to hide its existence
            }

            return View(emprunt);
        }

        // POST: Emprunt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emprunt = await _context.Emprunts
                .Include(e => e.Collegue)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (emprunt == null)
            {
                return NotFound();
            }

            // Verify the emprunt belongs to a collegue of the logged-in reparateur
            var reparateurId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (emprunt.Collegue.ReparateurId != reparateurId)
            {
                return Forbid(); // Or return a not found to hide its existence
            }

            _context.Emprunts.Remove(emprunt);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Collegue");
        }





        private bool EmpruntExists(int id)
        {
            return _context.Emprunts.Any(e => e.Id == id);
        }
    }
}