using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPageController : Controller
    {
        public IActionResult PageAdmin()
        {
            return View();
        }
    }
}