using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto, HttpContext);
        if (!result.IsSuccessful)
        {
            return result.Message == "User is already logged in" ? Conflict(result.Message) : Unauthorized(result.Message);
        }

        return Ok(new { message = result.Message, isAdmin = result.IsAdmin });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        if (!_authService.IsLoggedIn(HttpContext))
        {
            return BadRequest("No user logged in");
        }

        _authService.Logout(HttpContext);
        return Ok(new { message = "Logout successful" });
    }

    [HttpGet("loggedIn")]
    public IActionResult IsLoggedIn()
    {
        return Ok(_authService.IsLoggedIn(HttpContext));
    }
}

public class LoginDto
{

    public string? Email { get; set; }
    public string? Password { get; set; }
}
