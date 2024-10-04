using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext and configure SQLite
builder.Services.AddDbContext<Database>(options => options.UseSqlite("Data Source=database.db"));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure app URLs
app.Urls.Add("http://localhost:5002");

// Call the database initializer
DatabaseInitializer.InitializeDatabase();

// Define routes
app.MapGet("", () => "Hello");

// Add a new endpoint to interact with the database
app.MapGet("/adminlist", async () =>
{
    using (var connection = new SqliteConnection("Data Source=database.db"))
    {
        await connection.OpenAsync();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = @"
                SELECT id, user_name, email, is_admin
                FROM admin;
            ";

            using (var reader = await command.ExecuteReaderAsync())
            {
                var users = new List<object>();
                while (await reader.ReadAsync())
                {
                    var user = new
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1),
                        email = reader.GetString(2),
                        is_admin = reader.GetString(3)
                    };
                    users.Add(user);
                }

                return Results.Ok(users);
            }
        }
    }
});

// Use CORS and routing
app.UseCors("AllowReactApp");
app.UseRouting();
app.MapControllers();

// Start the application
app.Run();