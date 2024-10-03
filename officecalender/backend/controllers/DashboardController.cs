using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;


[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
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
                            SELECT id, user_name, email, is_admin 
                            FROM admin WHERE id IS 2;
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
                                    name = reader.GetString(1),
                                    email = reader.GetString(2),
                                    is_admin = reader.GetBoolean(3) // Use GetBoolean for is_admin
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
