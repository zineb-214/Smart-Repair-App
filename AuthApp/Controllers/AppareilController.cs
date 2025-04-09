using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AuthApp.Models;
using AuthApp.Data;
using System.Threading.Tasks;

namespace AuthApp.Controllers
{
    public class AppareilController : Controller
    {
        private readonly Context _context;

        public AppareilController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Appareil/Create
        public IActionResult Create(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Nom");
            return View();
        }

        // POST: Appareil/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,CategorieId")] Appareil appareil, string returnUrl = null)
        {
            returnUrl = returnUrl ?? TempData["ReturnUrl"]?.ToString() ?? Url.Action("Index", "Reparation");

            if (ModelState.IsValid)
            {
                _context.Add(appareil);
                await _context.SaveChangesAsync();
                return Redirect(returnUrl);
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Nom", appareil.CategorieId);
            ViewData["ReturnUrl"] = returnUrl;
            return View(appareil);
        }
    }
}