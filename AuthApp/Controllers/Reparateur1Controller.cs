using AuthApp.Data;
using AuthApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.Controllers
{
    public class Reparateur1Controller : Controller
    {
        private readonly Context _context;

        public Reparateur1Controller(Context context)
        {
            _context = context;
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
