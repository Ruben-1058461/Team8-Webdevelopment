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



// Start the application
app.Run();