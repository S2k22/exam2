using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.Data.Sqlite;

namespace Data
{
    public class CourseRepository
    {
        public async Task AddAsync(Course course)
        {
            try
            {
                await using var connection = Database.GetConnection();
                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = @"
                    INSERT INTO Courses (Title, Credits) 
                    VALUES (@Title, @Credits);";
                command.Parameters.AddWithValue("@Title", course.Title);
                command.Parameters.AddWithValue("@Credits", course.Credits);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding course: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Course>> GetAllAsync()
        {
            var courses = new List<Course>();
            try
            {
                await using var connection = Database.GetConnection();
                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Courses;";
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    courses.Add(new Course
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Credits = reader.GetInt32(2)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching courses: {ex.Message}");
                throw;
            }

            return courses;
        }

        public async Task<List<Course>> SearchAsync(string query)
        {
            var courses = new List<Course>();
            try
            {
                await using var connection = Database.GetConnection();
                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Courses WHERE Title LIKE @Query;";
                command.Parameters.AddWithValue("@Query", $"%{query}%");
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    courses.Add(new Course
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Credits = reader.GetInt32(2)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching courses: {ex.Message}");
                throw;
            }

            return courses;
        }

        public async Task DeleteAsync(int courseId)
        {
            try
            {
                await using var connection = Database.GetConnection();
                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM Courses WHERE Id = @Id;";
                command.Parameters.AddWithValue("@Id", courseId);
                var rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"No course found with ID {courseId}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting course: {ex.Message}");
                throw;
            }
        }

        public async Task<Course?> GetByIdAsync(int courseId)
        {
            try
            {
                await using var connection = Database.GetConnection();
                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Courses WHERE Id = @Id;";
                command.Parameters.AddWithValue("@Id", courseId);
                await using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new Course
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Credits = reader.GetInt32(2)
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching course by ID: {ex.Message}");
                throw;
            }

            return null;
        }

        public async Task<bool> TitleExistsAsync(string title)
        {
            try
            {
                await using var connection = Database.GetConnection();
                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(1) FROM Courses WHERE Title = @Title;";
                command.Parameters.AddWithValue("@Title", title);

                var count = (long)await command.ExecuteScalarAsync();
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking course title existence: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAsync(Course course)
        {
            try
            {
                await using var connection = Database.GetConnection();
                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE Courses 
                    SET Title = @Title, Credits = @Credits 
                    WHERE Id = @Id;";
                command.Parameters.AddWithValue("@Id", course.Id);
                command.Parameters.AddWithValue("@Title", course.Title);
                command.Parameters.AddWithValue("@Credits", course.Credits);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"No course found with ID {course.Id}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating course: {ex.Message}");
                throw;
            }
        }

        public async Task<List<(string Title, int EnrollmentCount)>> GetPopularCoursesAsync(int minEnrollment)
        {
            var popularCourses = new List<(string Title, int EnrollmentCount)>();
            try
            {
                await using var connection = Database.GetConnection();
                await connection.OpenAsync();

                await using var command = connection.CreateCommand();
                command.CommandText = @"
                    SELECT c.Title, COUNT(e.Id) AS EnrollmentCount
                    FROM Courses c
                    JOIN Enrollments e ON c.Id = e.CourseId
                    GROUP BY c.Title
                    HAVING EnrollmentCount >= @MinEnrollment;";
                command.Parameters.AddWithValue("@MinEnrollment", minEnrollment);

                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    popularCourses.Add((reader.GetString(0), reader.GetInt32(1)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching popular courses: {ex.Message}");
                throw;
            }

            return popularCourses;
        }
    }
}
