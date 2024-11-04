using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;


[Route("api/[controller]")]
[ApiController] 
public class UserlistController : ControllerBase
{
    private readonly string _connectionString = "Data Source=database.db"; // Set your connection string here

    [HttpGet]

    public async Task<IActionResult> GetUsersAsync()
    {
        try
        {
            var users = new List<object>();

            // Open a connection with SQLite using the provided connection string
            using (var connection = new SqliteConnection(_connectionString))
            {
                // Open the connection
                await connection.OpenAsync();

                // Create a new command and set the SQL query
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                            SELECT id, first_name, last_name, password, email, recurring_days, is_admin 
                            FROM user WHERE is_admin IS 0;
                        ";

                    // Execute the command and read the results
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Loop through the data returned by the query
                        while (await reader.ReadAsync())
                        {
                            var user = new
                            {
                                id = reader.GetInt32(0), // Index-based retrieval
                                first_name = reader.GetString(1),
                                last_name = reader.GetString(2),
                                password = reader.GetString(3), 
                                email = reader.GetString(4), 
                                recurring_days = reader.GetString(5), 
                                is_admin = reader.GetBoolean(6) // Use GetBoolean for is_admin
                            };
                            users.Add(user);
                        }
                    }
                }
            }

            // Return the list of users as a result
            return Ok(users);
        }
        catch (Exception ex)
        {
            // Handle errors and return a problem result
            return Problem("An error occurred while fetching users: " + ex.Message);
        }
    }
}