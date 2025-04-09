using AuthApp.Data;
using AuthApp.Models;
using AuthApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using X.PagedList;

namespace AuthApp.Controllers
{
    public class ClientController : BaseController
    {
        private readonly Context _context;
        private readonly ILogger<ClientController> _logger;

        public ClientController(
            Context context,
            JwtService jwtService,
            ILogger<ClientController> logger)
            : base(jwtService, logger)
        {
            _context = context;
            _logger = logger;
        }




        [Authorize(Roles = "Reparateur")]
        public async Task<IActionResult> Index(int? page)
        {
            var (userId, _, _) = GetCurrentUser();
            if (!int.TryParse(userId, out int reparateurId))
            {
                return RedirectToLogin();
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;

            var clients = await _context.Clients
                .Where(c => c.ReparateurId == reparateurId)
                .Include(c => c.Reparateur)
                .OrderBy(c => c.Nom)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(clients);
        }



        [Authorize(Roles = "Reparateur")]
        public IActionResult Create()
        {
            var (userId, _, _) = GetCurrentUser();
            if (!int.TryParse(userId, out int reparateurId))
            {
                return RedirectToLogin();
            }

            ViewBag.ReparateurId = reparateurId;
            ViewBag.ReparateurName = _context.Reparateurs
                .FirstOrDefault(r => r.reparateurID == reparateurId)?.Nom ?? "Your Account";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Reparateur")]
        public async Task<IActionResult> Create(Client client)
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
                    client.ReparateurId = reparateurId;
                    client.Code = GenerateUniqueCode();
                    UploadImage(client);

                    _context.Add(client);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating client");
                    ModelState.AddModelError("", "An error occurred while saving.");
                }
            }

            ViewBag.ReparateurId = reparateurId;
            return View(client);
        }

        [Authorize(Roles = "Reparateur")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var (userId, _, _) = GetCurrentUser();
            if (!int.TryParse(userId, out int reparateurId))
            {
                return RedirectToLogin();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == id && c.ReparateurId == reparateurId);

            if (client == null) return NotFound();

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Reparateur")]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id) return NotFound();

            var (userId, _, _) = GetCurrentUser();
            if (!int.TryParse(userId, out int reparateurId))
            {
                return RedirectToLogin();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Verify client belongs to this reparateur before update
                    var existingClient = await _context.Clients
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Id == id && c.ReparateurId == reparateurId);

                    if (existingClient == null) return NotFound();

                    client.ReparateurId = reparateurId; 
                    UploadImage(client);

                    _context.Update(client);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError(ex, "Concurrency error editing client");
                    if (!ClientExists(client.Id)) return NotFound();
                    throw;
                }
            }
            return View(client);
        }

        [Authorize(Roles = "Reparateur")]
        public async Task<IActionResult> Details(int? id)
        {
            // Authentication Check
            var (userId, _, _) = GetCurrentUser();
            if (!int.TryParse(userId, out int reparateurId))
            {
                _logger.LogWarning("Unauthorized access attempt");
                return RedirectToLogin();
            }

            // Parameter Validation
            if (id == null)
            {
                _logger.LogWarning("Details requested without ID");
                return NotFound();
            }

            // Query with Related Data
            var client = await _context.Clients
                .Include(c => c.Reparateur)
                .Include(c => c.Reparations)
                    .ThenInclude(r => r.DetailsReparations) // Now this will work
                .FirstOrDefaultAsync(c => c.Id == id && c.ReparateurId == reparateurId);

            // Not Found Handling
            if (client == null)
            {
                _logger.LogWarning($"Client {id} not found for reparateur {reparateurId}");
                return NotFound();
            }

            // Calculate Statistics
            ViewBag.TotalReparations = client.Reparations?.Count ?? 0;
            ViewBag.TotalCost = client.Reparations?
                .Sum(r => r.DetailsReparations?.Sum(d => d.PrixTotalReparation) ?? 0) ?? 0;
            ViewBag.TotalAdvance = client.Reparations?
                .Sum(r => r.DetailsReparations?.Sum(d => d.PrixAvance) ?? 0) ?? 0;
            ViewBag.TotalRemaining = client.Reparations?
                .Sum(r => r.DetailsReparations?.Sum(d => d.PrixReste) ?? 0) ?? 0;

            return View(client);
        }



        // GET: Client/Search
        public IActionResult Search()
        {
            return View();
        }

        // POST: Client/Search
        [HttpPost]
        public async Task<IActionResult> Search(int code)
        {
            // Fetch the client based on the code de suivi
            var client = await _context.Clients
                .Where(c => c.Code == code)
                .Include(c => c.Reparations)  // Load related reparations
                    .ThenInclude(r => r.DetailsReparations)
                     .ThenInclude(d => d.Appareil)

                .FirstOrDefaultAsync();

            if (client == null)
            {
                // No client found with that code
                TempData["ErrorMessage"] = "Client not found with the given Code de Suivi.";
                return View();
            }

            // Pass client and reparations to the view
            return View(client);
        }




        [Authorize(Roles = "Reparateur")]
        public async Task<IActionResult> Delete(int? id)
        {
            // Authentication and parameter validation
            var (userId, _, _) = GetCurrentUser();
            if (!int.TryParse(userId, out int reparateurId))
            {
                _logger.LogWarning("Unauthorized delete attempt");
                return RedirectToLogin();
            }

            if (id == null)
            {
                _logger.LogWarning("Delete requested without ID");
                return NotFound();
            }

            // Get client with related data for confirmation view
            var client = await _context.Clients
                .Include(c => c.Reparations)
                    .ThenInclude(r => r.DetailsReparations)
                .FirstOrDefaultAsync(c => c.Id == id && c.ReparateurId == reparateurId);

            if (client == null)
            {
                _logger.LogWarning($"Client {id} not found for reparateur {reparateurId}");
                return NotFound();
            }

            // Calculate statistics to show impact of deletion
            ViewBag.TotalReparations = client.Reparations?.Count ?? 0;
            ViewBag.TotalActiveRepairs = client.Reparations?
                .Count(r => r.DetailsReparations?.Any(d => d.Statut != "Completed") ?? false) ?? 0;

            return View(client);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Reparateur")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var (userId, _, _) = GetCurrentUser();
            if (!int.TryParse(userId, out int reparateurId))
            {
                _logger.LogWarning("Unauthorized delete attempt");
                return RedirectToLogin();
            }

            // Get client with all related data for cascade delete
            var client = await _context.Clients
                .Include(c => c.Reparations)
                    .ThenInclude(r => r.DetailsReparations)
                .FirstOrDefaultAsync(c => c.Id == id && c.ReparateurId == reparateurId);

            if (client == null)
            {
                return NotFound();
            }

            try
            {
                // First delete all repair details
                foreach (var reparation in client.Reparations)
                {
                    _context.DetailsReparations.RemoveRange(reparation.DetailsReparations);
                }

                // Then delete all reparations
                _context.Reparations.RemoveRange(client.Reparations);

                // Finally delete the client
                _context.Clients.Remove(client);

                await _context.SaveChangesAsync();

                // Delete associated image file if exists
                if (!string.IsNullOrEmpty(client.Image))
                {
                    var imagePath = Path.Combine("wwwroot", "Images", client.Image);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                TempData["SuccessMessage"] = $"Client {client.Nom} deleted successfully";
                _logger.LogInformation($"Client {id} deleted by reparateur {reparateurId}");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"Error deleting client {id}");
                TempData["ErrorMessage"] = $"Could not delete client {client.Nom}. They may have active repairs.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }




        private void UploadImage(Client client)
        {
            var file = HttpContext.Request.Form.Files;
            if (file.Count() > 0)
            {
                String imagename = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                var fileStream = new FileStream(Path.Combine(@"wwwroot/", "Images", imagename), FileMode.Create);
                file[0].CopyTo(fileStream);
                client.Image = imagename;
            }
            
        }

        private int GenerateUniqueCode()
        {
            int newCode;
            do
            {
                newCode = new Random().Next(100000000, 999999999);
            } while (_context.Clients.Any(c => c.Code == newCode));

            return newCode;
        }

        private bool ClientExists(int id) => _context.Clients.Any(e => e.Id == id);
    }
}