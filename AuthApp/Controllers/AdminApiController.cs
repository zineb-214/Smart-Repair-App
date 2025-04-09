using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using AuthApp.Data;
using AuthApp.Models;
using AuthApp.Services;

namespace AuthApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly Context _context;
        private readonly JwtService _jwtService;

        // Add JwtService to the constructor
        public AdminApiController(Context context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                // Recherche dans les trois types d'utilisateurs
                var admin = await _context.Admins
                    .FirstOrDefaultAsync(u => u.Username == loginDto.Username &&
                                            u.Password == loginDto.Password);

                if (admin != null)
                {
                    var token = _jwtService.GenerateToken(
                        admin.Id.ToString(),
                        admin.Username,
                        "Admin"); // <-- Ajout du rôle

                    return Ok(new { Token = token, User = admin });
                }
                var magasin = await _context.Magasins
    .FirstOrDefaultAsync(m => m.UserName == loginDto.Username &&
                            m.Password == loginDto.Password);

                if (magasin != null)
                {
                    var token = _jwtService.GenerateToken(
                        magasin.Id.ToString(),
                        magasin.UserName,
                        "Magasin");

                    return Ok(new { Token = token, User = magasin });
                }

                var reparateur = await _context.Reparateurs
                    .FirstOrDefaultAsync(r => r.UserName == loginDto.Username &&
                                            r.Password == loginDto.Password);

                if (reparateur != null)
                {
                    var token = _jwtService.GenerateToken(
                        reparateur.reparateurID.ToString(),
                        reparateur.UserName,
                        "Reparateur");

                    return Ok(new { Token = token, User = reparateur });
                }
                // Ajouter la logique pour Magasin/Reparateur si nécessaire
                return Unauthorized("Invalid credentials");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}