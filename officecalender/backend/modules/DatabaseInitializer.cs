using System;
using System.IO;
using Microsoft.Data.Sqlite;

public class DatabaseInitializer
{
    private const string DatabaseFileName = "database.db";

    public static void InitializeDatabase()
    {
        // Check if the database file already exists
        if (!File.Exists(DatabaseFileName))
        {
            // Create the database file
            using (var connection = new SqliteConnection($"Data Source={DatabaseFileName}"))
            {
                connection.Open();

                // Create tables
                CreateTables(connection);

                connection.Close();
                Console.WriteLine("Database created successfully.");
            }
        }
        else
        {
            Console.WriteLine("");
        }
    }

    private static void CreateTables(SqliteConnection connection)
    {
        string createAdminTable = @"
            CREATE TABLE IF NOT EXISTS admin (
                id INTEGER NOT NULL UNIQUE,
                user_name TEXT NOT NULL,
                password TEXT NOT NULL,
                email TEXT,
                is_admin INTEGER DEFAULT 1,
                PRIMARY KEY(id AUTOINCREMENT)
            );";

        string createAttendanceDataTable = @"
            CREATE TABLE IF NOT EXISTS attendance_data (
                id INTEGER NOT NULL UNIQUE,
                user_id INTEGER,
                date TEXT,
                PRIMARY KEY(id AUTOINCREMENT)
            );";

        string createEventAttendanceDataTable = @"
            CREATE TABLE IF NOT EXISTS event_attendance_data (
                id INTEGER NOT NULL UNIQUE,
                user_id INTEGER,
                event_id INTEGER,
                rating TEXT,
                feedback TEXT,
                PRIMARY KEY(id AUTOINCREMENT)
            );";

        string createEventDataTable = @"
            CREATE TABLE IF NOT EXISTS event_data (
                id INTEGER NOT NULL UNIQUE,
                title TEXT,
                description TEXT,
                date TEXT,
                start_time TEXT,
                end_time TEXT,
                location TEXT,
                admin_approval TEXT,
                PRIMARY KEY(id)
            );";

        string createUserTable = @"
            CREATE TABLE IF NOT EXISTS user (
                id INTEGER NOT NULL UNIQUE,
                first_name TEXT NOT NULL,
                last_name TEXT NOT NULL,
                password TEXT NOT NULL,
                email TEXT,
                recurring_days TEXT,
                is_admin INTEGER DEFAULT 0,
                PRIMARY KEY(id AUTOINCREMENT)
            );";

        // Execute each table creation query
        ExecuteNonQuery(connection, createAdminTable);
        ExecuteNonQuery(connection, createAttendanceDataTable);
        ExecuteNonQuery(connection, createEventAttendanceDataTable);
        ExecuteNonQuery(connection, createEventDataTable);
        ExecuteNonQuery(connection, createUserTable);
    }

    private static void ExecuteNonQuery(SqliteConnection connection, string query)
    {
        using (var command = new SqliteCommand(query, connection))
        {
            command.ExecuteNonQuery();
        }
    }
}