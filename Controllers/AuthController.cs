using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace EquipeTop14.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAdminService _adminService;
        private readonly IConfiguration _configuration;

        public AuthController(ILogger<AuthController> logger, IAdminService adminService, IConfiguration configuration)
        {
            _logger = logger;
            _adminService = adminService;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login([FromForm] string user, [FromForm][Required] string password)
        {
            _logger.LogInformation("Appel de la route : Login");
            _logger.LogInformation($"Tentative de connexion avec : user : {user}, password : {password}");

            if (!_adminService.Authenticate(user, password))
            {
                _logger.LogError("Mauvais renseignement de données de connexion");
                return Unauthorized("Erreur, le mot de passe et/ou l'utilisateur ne correspond pas");
            }

            string token = GenerateToken(user);

            var obj = new { token };
            _logger.LogInformation($"Connexion de l'utilisateur {user} réussie");
            return Ok(obj);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private string GenerateToken(string user)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> { new Claim("username", user) };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("admin")]
        [ProducesResponseType(typeof(AdminModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateAdmin([FromBody] AdminModel admin)
        {
            _logger.LogInformation("Appel de la route : CreateAdmin");

            try
            {
                var createdAdmin = _adminService.CreateAdmin(admin);
                _logger.LogInformation($"Administrateur {admin.Username} créé avec succès.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while creating an admin: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
