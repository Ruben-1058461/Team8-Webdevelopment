
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Database>(options => options.UseSqlite("Data Source=database.db"));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // Allow requests from this origin
               .AllowAnyHeader() // Allow any header
               .AllowAnyMethod(); // Allow any HTTP method (GET, POST, etc.)
    });
});




builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
var app = builder.Build();

// Add localhost and port
app.Urls.Add("http://localhost:5002");

// Call the database initializer
DatabaseInitializer.InitializeDatabase();


app.UseCors("AllowReactApp");

app.UseRouting();

app.MapControllers();
// Test hello
app.MapGet("", () => "Hello");
// adminlijst 
// adminlijst 

app.MapControllers();
app.MapControllers();


app.Run();

