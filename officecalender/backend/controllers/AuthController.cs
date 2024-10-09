using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt; // JwtSecurityTokenHandler
using officecalender.backend.Models; // Import the Models namespace

namespace officecalender.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Database _context;
        private readonly IConfiguration _configuration; // Add configuration to access appsettings

        public AuthController(Database context, IConfiguration configuration) // Include configuration in constructor
        {
            _context = context;
            _configuration = configuration; // Initialize configuration
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Find the user by email
            var user = await _context.Users.SingleOrDefaultAsync(u => u.email == request.Email);

            // Check if the user exists and if the password is correct
            if (user == null || !VerifyPassword(request.Password, user.password))
            {
                return Unauthorized("Invalid credentials");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); // Retrieve the key from appsettings
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = tokenHandler.WriteToken(token) });
        }

        // Verify Password Method (implementation assumed)
        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            // Implement your password verification logic here
            return inputPassword == storedHash; // placeholder; implement actual verification
        }
    }
}
