using AuthApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthApp.Data;

namespace AuthApp.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly Context _context;

        public AdminController(Context context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            int nombreAppareils = await _context.Appareils.CountAsync();
            var magasins = await _context.Magasins.ToListAsync();

            // Récupérer les appareils regroupés par catégorie
            var appareilsParCategorie = await _context.Appareils
                .GroupBy(a => a.Categorie.Nom) // Suppose que l'entité a une relation avec Categorie
                .Select(group => new { Categorie = group.Key, Nombre = group.Count() })
                .OrderBy(g => g.Categorie) // Trier par ordre alphabétique
                .ToListAsync();

            // Regrouper les magasins par date de création
            var magasinsParDate = magasins
                .GroupBy(m => m.Created_at.Date.ToString("yyyy-MM-dd"))
                .Select(group => new { Date = group.Key, NombreMagasins = group.Count() })
                .OrderBy(g => g.Date)
                .ToList();

            ViewBag.NombreAppareils = nombreAppareils;
            ViewBag.NombreMagasins = magasins.Count;

            // Ajouter les données pour le graphique en anneau
            ViewBag.LabelsAppareils = appareilsParCategorie.Select(a => a.Categorie).ToList();
            ViewBag.CountsAppareils = appareilsParCategorie.Select(a => a.Nombre).ToList();

            ViewBag.Dates = magasinsParDate.Select(m => m.Date).ToList();
            ViewBag.Counts = magasinsParDate.Select(m => m.NombreMagasins).ToList();

            ViewBag.ShowNavbar = false;

            var NombreCategories = await _context.Categories.CountAsync();
            ViewBag.NombreCategories = NombreCategories;

            return View();
        }




        [HttpGet("chart")]
        public async Task<IActionResult> Chart()
        {
            var magasins = await _context.Magasins.ToListAsync();
            var magasinsParVille = magasins
                .GroupBy(m => m.Ville)
                .Select(group => new { Ville = group.Key, NombreMagasins = group.Count() })
                .OrderByDescending(x => x.NombreMagasins)
                .ToList();

            ViewBag.Villes = magasinsParVille.Select(m => m.Ville).ToList();
            ViewBag.Counts = magasinsParVille.Select(m => m.NombreMagasins).ToList();
            ViewBag.ShowNavbar = false;




            var stats = await _context.Abonnement_Magasins
    .GroupBy(a => a.Statut)
    .Select(g => new { Statut = g.Key, Count = g.Count() })
    .ToListAsync();

            ViewBag.StatsLabels = stats.Select(s => s.Statut).ToArray();
            ViewBag.StatsData = stats.Select(s => s.Count).ToArray();


            return View();
        }

        [HttpGet("tables")]
        public async Task<IActionResult> Tables()
        {
            ViewBag.ShowNavbar = false;
            return View(await _context.Magasins.ToListAsync());
        }
        //Appareils
        [HttpGet("appareils")]
        public async Task<IActionResult> Appareils()
        {
            var appareils = await _context.Appareils.Include(a => a.Categorie).ToListAsync();
            ViewBag.ShowNavbar = false;
            return View(appareils);
        }

        [HttpGet("appareils/details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var appareil = await _context.Appareils.Include(a => a.Categorie).FirstOrDefaultAsync(a => a.Id == id);
            if (appareil == null) return NotFound();
            ViewBag.ShowNavbar = false;
            return View(appareil);
        }

        [HttpGet("appareils/create")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Nom");
            ViewBag.ShowNavbar = false;
            return View();
        }

        [HttpPost("appareils/create")]

        public async Task<IActionResult> Create(Appareil appareil)
        {
            if (ModelState.IsValid)
            {
                _context.Appareils.Add(appareil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Appareils));
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Nom"); // Recharge les catégories si erreur
            ViewBag.ShowNavbar = false;
            return View(appareil);
        }

        [HttpGet("appareils/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var appareil = await _context.Appareils.FindAsync(id);
            if (appareil == null) return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Nom", appareil.CategorieId);
            ViewBag.ShowNavbar = false;
            return View(appareil);
        }

        [HttpPost("appareils/edit/{id}")]
        public async Task<IActionResult> Edit(int id, Appareil appareil)
        {
            if (id != appareil.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                _context.Update(appareil);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Appareils));
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Nom", appareil.CategorieId);
            ViewBag.ShowNavbar = false;
            return View(appareil);
        }


        [HttpGet("appareils/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appareil = await _context.Appareils.FindAsync(id);
            if (appareil == null) return NotFound();
            ViewBag.ShowNavbar = false;
            return View(appareil);
        }

        [HttpPost("appareils/delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appareil = await _context.Appareils.FindAsync(id);
            if (appareil == null) return NotFound();

            _context.Appareils.Remove(appareil);
            await _context.SaveChangesAsync();
            ViewBag.ShowNavbar = false;
            return RedirectToAction(nameof(Appareils));
        }

        [HttpGet("catégories")]
        public async Task<IActionResult> Catégories()
        {
            ViewBag.ShowNavbar = false;
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        [HttpGet("admin/createctg")]
        public IActionResult Createctg()
        {
            return View();
        }

        // Ajouter une nouvelle catégorie
        [HttpPost("admin/createctg")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Createctg(Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(categorie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Catégories));
            }
            return View(categorie);
        }

        // Afficher le formulaire de modification
        [HttpGet("admin/editctg/{id}")]
        public async Task<IActionResult> Editctg(int id)
        {
            var categorie = await _context.Categories.FindAsync(id);
            if (categorie == null)
            {
                return NotFound();
            }
            return View(categorie);
        }

        // Modifier une catégorie
        [HttpPost("admin/editctg/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editctg(int id, Categorie categorie)
        {
            if (id != categorie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categorie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Categories.Any(c => c.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Catégories));
            }
            return View(categorie);
        }

        // Supprimer une catégorie
        [HttpGet("admin/deletectg/{id}")]
        public async Task<IActionResult> Deletectg(int id)
        {
            var categorie = await _context.Categories.FindAsync(id);
            if (categorie == null)
            {
                return NotFound();
            }
            return View(categorie);
        }

        // Confirmation de la suppression
        [HttpPost("admin/deletectg/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedctg(int id)
        {
            var categorie = await _context.Categories.FindAsync(id);
            if (categorie != null)
            {
                _context.Categories.Remove(categorie);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Catégories));
        }
    }

}
