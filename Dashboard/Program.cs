using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var app = builder.Build();


// Add a new endpoint to interact with the database
app.MapGet("/Dashboard", async () =>
{
    // Open a connection with SQLite using the provided connection string
    using (var connection = new SqliteConnection($"Data Source=database.db"))
    {
        // Open the connection
        await connection.OpenAsync();

        // Create a new command and set the SQL query
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
                SELECT id, user_name,email,is_admin
                FROM admin;
            ";

            // Execute the command and read the results
            using (var reader = await command.ExecuteReaderAsync())
            {
                var users = new List<object>();

                // Loop through the data returned by the query
                while (await reader.ReadAsync())
                {
                    var user = new
                    {
                        id = reader.GetInt32(0), // Index-based retrieval
                        name = reader.GetString(1),
                        email = reader.GetString(2),
                        adminstatus = reader.GetString(3)

                    };
                    users.Add(user);
                }

                // Return the list of users as a result
                return Results.Ok(users);
            }
        }
    }
});


app.Run();
