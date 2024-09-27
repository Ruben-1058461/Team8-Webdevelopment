var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
var app = builder.Build();

// Add localhost and port
app.Urls.Add("http://localhost:5002");

// Test hello
app.MapGet("" , ()=> "Hello");





app.Run();

