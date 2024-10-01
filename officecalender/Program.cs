using Microsoft.Data.Sqlite;
 
 
 
var builder = WebApplication.CreateBuilder(args);
 
 
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
var app = builder.Build();
 
// Add localhost and port
app.Urls.Add("http://localhost:5002");
 
// Call the database initializer
DatabaseInitializer.InitializeDatabase();
 
// Test hello
app.MapGet("", () => "Hello");
 
 
// Add a new endpoint to interact with the database
app.MapGet("/adminlist", async () =>
{
    // Open a connection with SQLite using the provided connection string
    using (var connection = new SqliteConnection("Data Source=database.db"))
    {
        // Open the connection
        await connection.OpenAsync();
 
        // Create a new command and set the SQL query
        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
                SELECT id, user_name,email, is_admin
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
                        is_admin = reader.GetString(3)
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