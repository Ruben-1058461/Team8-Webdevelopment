using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure database connection from appsettings.json
builder.Services.AddDbContext<Database>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); // Ensure this connection is defined in appsettings.json

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

// Add session services
builder.Services.AddDistributedMemoryCache(); // Required for session state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make cookie accessible only by the server
    options.Cookie.IsEssential = true; // Required for session to work
});

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<LoggedInAttribute>(); // Apply the LoggedIn filter globally
});

builder.Services.AddEndpointsApiExplorer();

// Configure Swagger for API documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// Middleware
app.UseAuthentication();
app.UseAuthorization();

// Use session middleware
app.UseSession();

// Enable middleware to serve generated Swagger as a JSON endpoint
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = string.Empty; // Set the Swagger UI at the app's root
});

// Use CORS policy
app.UseCors("AllowReactApp");

// Use filters
app.Use(async (context, next) =>
{
    await next.Invoke();
});

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

// Map controllers
app.MapControllers();

// Run the application
app.Run();
