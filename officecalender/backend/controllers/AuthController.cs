using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace officecalender.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Database _context;

        public AuthController(Database context)
        {
            _context = context;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Find the user by first name
            var user = await _context.Users.SingleOrDefaultAsync(u => u.email == request.Email);


            // Check if the user exists and if the password is correct
            if (user == null || !VerifyPassword(request.Password, user.password, user.salt))
            {
                return Unauthorized("Invalid credentials");
            }

            // Generate token
            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        // Verify
        private bool VerifyPassword(string password, string storedHash, byte[] salt)
        {
            // Salt
            using (var hmac = new HMACSHA256(salt))
            {
                var hashedInputPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashedInputPasswordString = Convert.ToBase64String(hashedInputPassword);

                // Compare the hashed input password with the stored hash
                return hashedInputPasswordString == storedHash;
            }
        }

        // Generate Token
        private string GenerateJwtToken(User user)
        {
            // Implement token
            return "your-generated-jwt-token";
        }
    }

    // Request model for login
    public class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
