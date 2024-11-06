using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly string _connectionString = "Data Source=database.db"; 
    [HttpGet]
    public async Task<IActionResult> GetUserAsync()
    {
        // Retrieve the UserId from the session
        int? userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
        {
            return Unauthorized("User ID not found in session; user might not be logged in.");
        }

        try
        {
            object user = null;
            bool isAdmin = false;

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Checks for admin status
                using (var checkAdminCommand = connection.CreateCommand())
                {
                    checkAdminCommand.CommandText = @"
                        SELECT is_admin
                        FROM admin
                        WHERE id = $userId;";
                    checkAdminCommand.Parameters.AddWithValue("$userId", userId.Value);

                    var isAdminResult = await checkAdminCommand.ExecuteScalarAsync();
                    isAdmin = isAdminResult != null && Convert.ToBoolean(isAdminResult);
                }

                // Checks for admin status then retrieves it
                using (var command = connection.CreateCommand())
                {
                    if (isAdmin)
                    {
                        command.CommandText = @"
                            SELECT id, first_name, last_name, email, is_admin 
                            FROM admin
                            WHERE id = $userId;";
                    }
                    else
                    {
                        command.CommandText = @"
                            SELECT id, first_name, last_name, email, recurring_days 
                            FROM user
                            WHERE id = $userId;";
                    }
                    command.Parameters.AddWithValue("$userId", userId.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            if (isAdmin)
                            {
                                user = new
                                {
                                    id = reader.GetInt32(0),
                                    first_name = reader.GetString(1),
                                    last_name = reader.GetString(2),
                                    email = reader.GetString(3),
                                    privileges = reader.GetString(4) 
                                };
                            }
                            else
                            {
                                user = new
                                {
                                    id = reader.GetInt32(0),
                                    first_name = reader.GetString(1),
                                    last_name = reader.GetString(2),
                                    email = reader.GetString(3),
                                    recurring_days = reader.GetInt32(4)
                                };
                            }
                        }
                    }
                }
            }

            if (user == null)
            {
                return NotFound("User not found.");
            }

            
            return Ok(user);
        }
        catch (Exception ex)
        {
            
            return Problem("An error occurred while fetching user data: " + ex.Message);
        }
    }
}