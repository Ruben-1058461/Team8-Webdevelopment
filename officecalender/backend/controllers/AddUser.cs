using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;


[Route("api/[controller]")]
[ApiController]
public class AddUserController : ControllerBase
{
    private readonly string _connectionString = "Data Source=database.db"; // Set your connection string here

    [HttpGet]

    public async Task<IActionResult> GetUsersAsync()
    {
        try
        {
            var users = new List<object>();
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

                    // Add parameters to avoid SQL injection
                    // command.Parameters.AddWithValue("@id", "1");
                    command.Parameters.AddWithValue("@first_name", "Rushil");
                    command.Parameters.AddWithValue("@last_name", "Lachmon");
                    command.Parameters.AddWithValue("@password", "abc");
                    command.Parameters.AddWithValue("@email", "1044025@hr.nl");
                    command.Parameters.AddWithValue("@recurring_days", "12");
                    command.Parameters.AddWithValue("@is_admin", "0");


                    // Execute the command asynchronously and check the result
                    int result = await command.ExecuteNonQueryAsync();


                    // Open the connection


                    // Create a new command and set the SQL query


                    // Return the list of users as a result
                    return Ok(users);
                }
            }
        }
        catch (Exception ex)
        {
            // Handle errors and return a problem result
            return Problem("An error occurred while fetching users: " + ex.Message);
        }
    }
}






/*

   // Open a connection with SQLite using the provided connection string
            using (var connection = new SqliteConnection(_connectionString))
            {

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                     

                    command.Parameters.AddWithValue("@id", "1");
                    command.Parameters.AddWithValue("@first_name", "Rushil");
                    command.Parameters.AddWithValue("@last_name", "Lachmon");
                    command.Parameters.AddWithValue("@password", "abc");
                    command.Parameters.AddWithValue("@email", "1044025@hr.nl");
                    command.Parameters.AddWithValue("@recurring_days", "12");
                    command.Parameters.AddWithValue("@is_admin", "0");

                    await connection.OpenAsync();
                    // Execute the command and read the results
                    int result = await command.ExecuteNonQueryAsync();
                   
                }
            }

*/