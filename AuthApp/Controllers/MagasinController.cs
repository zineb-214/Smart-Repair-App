using AuthApp.Data;
using AuthApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthApp.Controllers
{
    public class MagasinController : Controller
    {
        private readonly Context _context;

        public MagasinController(Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Magasin store)
        {
            if (ModelState.IsValid)
            {
                _context.Magasins.Add(store);
                await _context.SaveChangesAsync();

                TempData["MagasinId"] = store.Id;
                TempData["StoreName"] = store.Nom;
                return RedirectToAction("Createe", "Reparateur1", new { magasinId = store.Id });
            }

            return View(store);
        }

    }
}
