using Domain.Models;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Data
{
    public class StudentRepository
    {
        public void Add(Student student)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Students (Name, Email) VALUES (@Name, @Email);";
            command.Parameters.AddWithValue("@Name", student.Name);
            command.Parameters.AddWithValue("@Email", student.Email);
            command.ExecuteNonQuery();
        }

        public List<Student> GetAll()
        {
            var students = new List<Student>();
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Students;";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                students.Add(new Student
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2)
                });
            }

            return students;
        }

        public List<Student> Search(string query)
        {
            var students = new List<Student>();
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Students WHERE Name LIKE @Query OR Email LIKE @Query;";
            command.Parameters.AddWithValue("@Query", $"%{query}%");

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                students.Add(new Student
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2)
                });
            }

            return students;
        }

        public void Delete(int studentId)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Students WHERE Id = @Id;";
            command.Parameters.AddWithValue("@Id", studentId);
            var rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"No student found with ID {studentId}.");
            }
        }
        
        public Student? GetById(int studentId)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Students WHERE Id = @Id;";
            command.Parameters.AddWithValue("@Id", studentId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Student
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2)
                };
            }

            return null;
        }
        
        public bool EmailExists(string email)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM Students WHERE Email = @Email;";
            command.Parameters.AddWithValue("@Email", email);

            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        
        public void Update(Student student)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
        UPDATE Students 
        SET Name = @Name, Email = @Email 
        WHERE Id = @Id;";
            command.Parameters.AddWithValue("@Id", student.Id);
            command.Parameters.AddWithValue("@Name", student.Name);
            command.Parameters.AddWithValue("@Email", student.Email);

            var rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"No student found with ID {student.Id}.");
            }
        }

        public List<(string CourseTitle, string? Grade)> GetStudentPerformance(int studentId)
        {
            var performance = new List<(string, string?)>();
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
        SELECT c.Title, e.Grade
        FROM Enrollments e
        JOIN Courses c ON e.CourseId = c.Id
        WHERE e.StudentId = @StudentId;
    ";
            command.Parameters.AddWithValue("@StudentId", studentId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                performance.Add((reader.GetString(0), reader.IsDBNull(1) ? null : reader.GetString(1)));
            }

            return performance;
        }


    }
}
