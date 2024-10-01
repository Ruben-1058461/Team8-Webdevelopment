using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Database>(options => options.UseSqlite("Data Source=database.db"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
var app = builder.Build();

// Add localhost and port
app.Urls.Add("http://localhost:5002");

// Call the database initializer
DatabaseInitializer.InitializeDatabase();

// Test hello
app.MapGet("", () => "Hello");
// adminlijst 

app.MapControllers();


app.Run();

