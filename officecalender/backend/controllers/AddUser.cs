using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using BCrypt.Net; // Voeg deze regel toe om BCrypt te gebruiken

[Route("api/[controller]")]
[ApiController]
public class AddUserController : ControllerBase
{
    private readonly string _connectionString = "Data Source=database.db"; // Stel hier je connectiestring in

    // POST: api/AddUser
    [HttpPost]
    public async Task<IActionResult> AddUserAsync([FromBody] UserRegistrationRequest request)
    {
        try
        {
            // Create a new SQL connection
            using (var connection = new SqliteConnection(_connectionString))
            {
                // Open the connection asynchronously
                await connection.OpenAsync();

                // Create a new command for inserting a user
                using (var command = connection.CreateCommand())
                {
                    // Set the SQL query for inserting a new user
                    command.CommandText = @"
                    INSERT INTO user (first_name, last_name, password, email, recurring_days, is_admin)
                    VALUES (@first_name, @last_name, @password, @email, @recurring_days, @is_admin);
                    ";

                    // Hash het wachtwoord met BCrypt
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

                    // Voeg parameters toe om SQL-injectie te voorkomen
                    command.Parameters.AddWithValue("@first_name", request.FirstName);
                    command.Parameters.AddWithValue("@last_name", request.LastName);
                    command.Parameters.AddWithValue("@password", hashedPassword); // Gebruik het gehashte wachtwoord
                    command.Parameters.AddWithValue("@email", request.Email);
                    command.Parameters.AddWithValue("@recurring_days", request.RecurringDays);
                    command.Parameters.AddWithValue("@is_admin", request.IsAdmin);

                    // Voer de opdracht asynchroon uit
                    await command.ExecuteNonQueryAsync();

                    return Ok("User added successfully");
                }
            }
        }
        catch (Exception ex)
        {
            // Handle errors and return a problem result
            return Problem("An error occurred while adding the user: " + ex.Message);
        }
    }
}

// Request model for user registration
public class UserRegistrationRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public int RecurringDays { get; set; }
    public bool IsAdmin { get; set; }
}
