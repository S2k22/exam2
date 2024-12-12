using Microsoft.Data.Sqlite;

namespace Data
{
    public static class Database
    {
        private const string ConnectionString = "Data Source=students.db";

        public static SqliteConnection GetConnection() => new SqliteConnection(ConnectionString);

        public static void Initialize()
        {
            using var connection = GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Students (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE
                );

                CREATE TABLE IF NOT EXISTS Courses (
                    Id INTEGER PRIMARY KEY,
                    Title TEXT NOT NULL,
                    Credits INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Enrollments (
                    Id INTEGER PRIMARY KEY,
                    StudentId INTEGER NOT NULL,
                    CourseId INTEGER NOT NULL,
                    Grade TEXT,
                    EnrollmentDate TEXT NOT NULL,
                    FOREIGN KEY (StudentId) REFERENCES Students(Id) ON DELETE CASCADE,
                    FOREIGN KEY (CourseId) REFERENCES Courses(Id) ON DELETE CASCADE
                );
            ";
            command.ExecuteNonQuery();
        }
    }
}