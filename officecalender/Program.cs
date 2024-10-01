using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure database connection from appsettings.json
builder.Services.AddDbContext<Database>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger for API documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// Use CORS policy
app.UseCors("AllowAllOrigins");

// Configure exception handling
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Show detailed errors in development
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Show generic error page in production
    app.UseHsts(); // Enable HTTP Strict Transport Security
}

// Add localhost and port
app.Urls.Add("http://localhost:5002");

// Call the database initializer
DatabaseInitializer.InitializeDatabase();

// Test hello route
app.MapGet("", () => "Hello");

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

// Map controllers
app.MapControllers();

// Run the application
app.Run();
