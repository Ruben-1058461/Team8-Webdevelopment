using MySqlConnector;  // Make sure you include this at the top
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Registering MySQL connection string as a singleton (optional, can be scoped or transient as needed)
builder.Services.AddSingleton<MySqlConnectionStringBuilder>(sp => new MySqlConnectionStringBuilder
{
    Server = "localhost",
    UserID = "root",
    Password = "speedylight101",
    Database = "mydb"
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// Add a new endpoint to interact with the database
app.MapGet("/users", async (MySqlConnectionStringBuilder connectionStringBuilder) =>
{
    // Open a connection asynchronously
    using var connection = new MySqlConnection(connectionStringBuilder.ConnectionString);
    await connection.OpenAsync();

    // Create a DB command and set the SQL statement
    using var command = connection.CreateCommand();
    command.CommandText = @"SELECT * FROM Users;";

    // Execute the command and read the results
    using var reader = await command.ExecuteReaderAsync();
    var users = new List<object>();

    while (reader.Read())
    {
        var user = new
        {
            id = reader.GetInt32("user_id"),
            name = reader.GetString("userName")
        };
        users.Add(user);
    }

    return Results.Ok(users); // Return the list of users as JSON
});

app.Run();
