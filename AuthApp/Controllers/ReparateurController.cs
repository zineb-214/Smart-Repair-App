using AuthApp.Data;
using AuthApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthApp.Controllers
{
    [Authorize(Roles = "Magasin")]
    public class ReparateurController : Controller
    {
        private readonly Context _context;

        public ReparateurController(Context context)
        {
            _context = context;
        }

        // GET: Reparateur
        public async Task<IActionResult> Index()
        {
            var magasinId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var reparateurs = await _context.Reparateurs
                .Where(r => r.MagasinId == magasinId)
                .Include(r => r.Magasin)
                .ToListAsync();

            return View("~/Views/MagasinPage/PageMagasin.cshtml", reparateurs);
        }

        // GET: Reparateur/Create
        public IActionResult Create()
        {
            return View("~/Views/MagasinPage/Create.cshtml");
        }

        // POST: Reparateur/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("reparateurID,Nom,Role,TaxPer,UserName,Password")] Reparateur reparateur)
        {
            if (ModelState.IsValid)
            {
                reparateur.MagasinId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _context.Add(reparateur);
                await _context.SaveChangesAsync();
                return RedirectToAction("PageMagasin", "MagasinPage");

            }
            return View("~/Views/MagasinPage/Create.cshtml", reparateur);
        }

        // GET: Reparateur/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var magasinId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var reparateur = await _context.Reparateurs
                .FirstOrDefaultAsync(r => r.reparateurID == id && r.MagasinId == magasinId);

            if (reparateur == null) return NotFound();

            return View("~/Views/MagasinPage/Edit.cshtml", reparateur);
        }

        // POST: Reparateur/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("reparateurID,Nom,Role,TaxPer,UserName,Password,MagasinId")] Reparateur reparateur)
        {
            if (id != reparateur.reparateurID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reparateur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReparateurExists(reparateur.reparateurID))
                        return NotFound();
                    throw;
                }
                // Redirigez vers PageMagasin du MagasinPageController au lieu de Index
                return RedirectToAction("PageMagasin", "MagasinPage");
            }
            return View("~/Views/MagasinPage/Edit.cshtml", reparateur);
        }
        // GET: Affiche la page de confirmation de suppression
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var magasinId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var reparateur = await _context.Reparateurs
                .FirstOrDefaultAsync(r => r.reparateurID == id && r.MagasinId == magasinId);

            if (reparateur == null) return NotFound();

            return View("~/Views/MagasinPage/Delete.cshtml", reparateur);
        }

        // POST: Effectue la suppression
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var magasinId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var reparateur = await _context.Reparateurs
                .FirstOrDefaultAsync(r => r.reparateurID == id && r.MagasinId == magasinId);

            if (reparateur != null)
            {
                _context.Reparateurs.Remove(reparateur);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("PageMagasin", "MagasinPage");

        }
        private bool ReparateurExists(int id)
        {
            return _context.Reparateurs.Any(e => e.reparateurID == id);
        }



        [HttpGet]
        public IActionResult Createe()
        {
            ViewBag.StoreName = TempData["StoreName"];
            ViewBag.MagasinId = TempData["MagasinId"];
            TempData.Keep(); // Keep TempData alive for POST
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Createe(Reparateur reparateur)
        {
            if (TempData["MagasinId"] != null && ModelState.IsValid)
            {
                reparateur.MagasinId = (int)TempData["MagasinId"];
                _context.Reparateurs.Add(reparateur);
                await _context.SaveChangesAsync();

                return RedirectToAction("Sub", "Home");
            }

            return View(reparateur);
        }














    }
}
