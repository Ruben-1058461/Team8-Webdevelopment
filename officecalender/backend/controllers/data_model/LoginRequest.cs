namespace officecalender.backend.Models
{
    // Request model for login
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        // Constructor to ensure properties are initialized
        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
