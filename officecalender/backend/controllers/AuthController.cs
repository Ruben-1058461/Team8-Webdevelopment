using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly Database _context;
    private readonly IConfiguration _configuration;

    public AuthController(Database context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        // Check if user exists
        var user = _context.Users.FirstOrDefault(u => u.email == loginDto.Email);
        if (user == null)
        {
            return Unauthorized("Invalid email or password");
        }

        // Verify password
        if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.password))
        {
            return Unauthorized("Invalid email or password");
        }

        // Check if the user is already logged in
        if (HttpContext.Session.GetString("UserEmail") == user.email)
        {
            return Conflict("User is already logged in");
        }

        // Check if the user is an admin
        var isAdmin = user.is_admin;
        HttpContext.Session.SetString("is_admin", isAdmin ? "true" : "false");

        // Generate JWT token
        var token = GenerateJwtToken(user);

        try
        {
            // Store email, user ID, and admin status in the session
            HttpContext.Session.SetString("UserEmail", user.email);
            HttpContext.Session.SetInt32("UserId", user.id);

            // Verify session values
            var emailInSession = HttpContext.Session.GetString("UserEmail");
            var userIdInSession = HttpContext.Session.GetInt32("UserId");

            if (string.IsNullOrEmpty(emailInSession) || !userIdInSession.HasValue)
            {
                return StatusCode(500, "Failed to store session values");
            }

            return Ok(new { message = "Logged in Successfully", isAdmin });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Exception: {ex.Message}");
        }
    }


    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
        {
            return BadRequest("No user logged in");
        }
        // Clear the user session I also made it clear the ID
        HttpContext.Session.Remove("UserEmail");
        HttpContext.Session.Remove("UserId");
        return Ok(new { message = "Logout successful" });
    }

    [HttpGet("loggedIn")]
    public IActionResult IsLoggedIn()
    {
        // Check if there's an active session with a user email
        var userEmail = HttpContext.Session.GetString("UserEmail");

        if (!string.IsNullOrEmpty(userEmail))
        {
            return Ok(true);
        }
        return BadRequest(false);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginDto
{

    public string? Email { get; set; }
    public string? Password { get; set; }
}
