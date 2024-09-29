/*
using MySqlConnector;
using System;

Console.WriteLine("Hello, World!");
// set these values correctly for your database server
var builder = new MySqlConnectionStringBuilder
{
	Server = "localhost",
	UserID = "root",
	Password = "speedylight101",
	Database = "mydb",
};

// open a connection asynchronously
using var connection = new MySqlConnection(builder.ConnectionString);
await connection.OpenAsync();

// create a DB command and set the SQL statement with parameters
using var command = connection.CreateCommand();
command.CommandText = @"SELECT * FROM Users;";
//command.Parameters.AddWithValue("@idUsers", idUsers);

// execute the command and read the results
using var reader = await command.ExecuteReaderAsync();
while (reader.Read())
{
	var id = reader.GetInt32("user_id");
	var name = reader.GetString("userName");
    Console.WriteLine(id);
    Console.WriteLine(name);
	// ...
}
*/