using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class AuthService
{
    private readonly Database _context;
    private readonly IConfiguration _configuration;

    public AuthService(Database context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<(bool IsSuccessful, string Message, bool IsAdmin, string Token)> LoginAsync(LoginDto loginDto, HttpContext httpContext)
    {
        var user = _context.Users.FirstOrDefault(u => u.email == loginDto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.password))
        {
            return (false, "Invalid email or password", false, null);
        }

        if (httpContext.Session.GetString("UserEmail") == user.email)
        {
            return (false, "User is already logged in", user.is_admin, null);
        }

        var token = GenerateJwtToken(user);
        httpContext.Session.SetString("UserEmail", user.email);
        httpContext.Session.SetInt32("UserId", user.id);
        httpContext.Session.SetString("is_admin", user.is_admin ? "true" : "false");

        return (true, "Logged in Successfully", user.is_admin, token);
    }

    public void Logout(HttpContext httpContext)
    {
        httpContext.Session.Remove("UserEmail");
        httpContext.Session.Remove("UserId");
    }

    public bool IsLoggedIn(HttpContext httpContext)
    {
        return !string.IsNullOrEmpty(httpContext.Session.GetString("UserEmail"));
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
