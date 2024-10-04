namespace officecalender.backend.Models
{
    // Request model for login
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
