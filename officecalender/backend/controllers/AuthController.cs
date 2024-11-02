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

        // Generate JWT token
        var token = GenerateJwtToken(user);

        // Store user email in session or other method to check if already logged in
        HttpContext.Session.SetString("UserEmail", user.email);

        return Ok("Logged in Succesfully");

        // return this if you want the user to see the token
        // return Ok(new { token });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
        {
            return BadRequest("No user logged in");
        }
        // Clear the user session
        HttpContext.Session.Remove("UserEmail");
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
