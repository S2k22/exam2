using Domain.Models;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Data
{
    public class CourseRepository
    {
        public void Add(Course course)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Courses (Title, Credits) VALUES (@Title, @Credits);";
            command.Parameters.AddWithValue("@Title", course.Title);
            command.Parameters.AddWithValue("@Credits", course.Credits);
            command.ExecuteNonQuery();
        }

        public List<Course> GetAll()
        {
            var courses = new List<Course>();
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Courses;";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                courses.Add(new Course
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Credits = reader.GetInt32(2)
                });
            }

            return courses;
        }

        public List<Course> Search(string query)
        {
            var courses = new List<Course>();
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Courses WHERE Title LIKE @Query;";
            command.Parameters.AddWithValue("@Query", $"%{query}%");

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                courses.Add(new Course
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Credits = reader.GetInt32(2)
                });
            }

            return courses;
        }

        public void Delete(int courseId)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Courses WHERE Id = @Id;";
            command.Parameters.AddWithValue("@Id", courseId);
            var rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"No course found with ID {courseId}.");
            }
        }
        
        public Course? GetById(int courseId)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Courses WHERE Id = @Id;";
            command.Parameters.AddWithValue("@Id", courseId);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Course
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Credits = reader.GetInt32(2)
                };
            }

            return null;
        }
        
        public bool TitleExists(string title)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT COUNT(1) FROM Courses WHERE Title = @Title;";
            command.Parameters.AddWithValue("@Title", title);

            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        
        public void Update(Course course)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
        UPDATE Courses 
        SET Title = @Title, Credits = @Credits 
        WHERE Id = @Id;";
            command.Parameters.AddWithValue("@Id", course.Id);
            command.Parameters.AddWithValue("@Title", course.Title);
            command.Parameters.AddWithValue("@Credits", course.Credits);

            var rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                throw new InvalidOperationException($"No course found with ID {course.Id}.");
            }
        }
        
        public List<(string CourseTitle, int EnrollmentCount)> GetPopularCourses()
        {
            var popularCourses = new List<(string, int)>();
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
        SELECT c.Title, COUNT(e.Id) AS EnrollmentCount
        FROM Enrollments e
        JOIN Courses c ON e.CourseId = c.Id
        GROUP BY c.Title
        ORDER BY EnrollmentCount DESC;
    ";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                popularCourses.Add((reader.GetString(0), reader.GetInt32(1)));
            }

            return popularCourses;
        }


    }
}
