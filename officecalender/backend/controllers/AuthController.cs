using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly Database _context;

    public AuthController(Database context)
    {
        _context = context;
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

        // Success
        return Ok(new { message = "Login successful" });
    }
}

public class LoginDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}