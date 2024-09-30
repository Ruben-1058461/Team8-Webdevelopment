var builder = WebApplication.CreateBuilder(args);


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

app.MapControllers();


app.Run();

