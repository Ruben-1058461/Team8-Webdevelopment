using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserService
{
    private readonly string _connectionString = "Data Source=database.db";

    public async Task<List<object>> GetUsersAsync()
    {
        var users = new List<object>();

        using (var connection = new SqliteConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    SELECT id, first_name, last_name, password, email, recurring_days, is_admin 
                    FROM user WHERE is_admin IS 0;
                ";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var user = new
                        {
                            id = reader.GetInt32(0),
                            first_name = reader.GetString(1),
                            last_name = reader.GetString(2),
                            password = reader.GetString(3),
                            email = reader.GetString(4),
                            recurring_days = reader.GetString(5),
                            is_admin = reader.GetBoolean(6)
                        };
                        users.Add(user);
                    }
                }
            }
        }
        return users;
    }
}