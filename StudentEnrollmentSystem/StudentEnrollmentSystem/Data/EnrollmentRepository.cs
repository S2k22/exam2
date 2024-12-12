using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Domain.Models;

namespace Data
{
    public class EnrollmentRepository
    {
        public void Add(Enrollment enrollment)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Enrollments (StudentId, CourseId, Grade, EnrollmentDate) 
                VALUES (@StudentId, @CourseId, @Grade, @EnrollmentDate);";
            command.Parameters.AddWithValue("@StudentId", enrollment.StudentId);
            command.Parameters.AddWithValue("@CourseId", enrollment.CourseId);
            command.Parameters.AddWithValue("@Grade", enrollment.Grade ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@EnrollmentDate", enrollment.EnrollmentDate.ToString("yyyy-MM-dd HH:mm:ss"));
            command.ExecuteNonQuery();
        }

        public List<(Enrollment, string StudentName, string CourseTitle)> GetAllWithDetails()
        {
            var enrollments = new List<(Enrollment, string, string)>();
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT 
                    e.Id, e.StudentId, e.CourseId, e.Grade, e.EnrollmentDate,
                    s.Name AS StudentName,
                    c.Title AS CourseTitle
                FROM Enrollments e
                JOIN Students s ON e.StudentId = s.Id
                JOIN Courses c ON e.CourseId = c.Id;
            ";
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var enrollment = new Enrollment
                {
                    Id = reader.GetInt32(0),
                    StudentId = reader.GetInt32(1),
                    CourseId = reader.GetInt32(2),
                    Grade = reader.IsDBNull(3) ? null : reader.GetString(3),
                    EnrollmentDate = DateTime.Parse(reader.GetString(4))
                };

                var studentName = reader.GetString(5);
                var courseTitle = reader.GetString(6);

                enrollments.Add((enrollment, studentName, courseTitle));
            }

            return enrollments;
        }

        public void Delete(int enrollmentId)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Enrollments WHERE Id = @Id;";
            command.Parameters.AddWithValue("@Id", enrollmentId);
            command.ExecuteNonQuery();
        }
        
        public bool EnrollmentExists(int studentId, int courseId)
        {
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
               SELECT COUNT(1) 
               FROM Enrollments 
               WHERE StudentId = @StudentId AND CourseId = @CourseId;";
            command.Parameters.AddWithValue("@StudentId", studentId);
            command.Parameters.AddWithValue("@CourseId", courseId);

            return Convert.ToInt32(command.ExecuteScalar()) > 0;
        }
        
        public List<(string StudentName, string StudentEmail)> GetEnrollmentsByCourse(int courseId)
        {
            var students = new List<(string, string)>();
            using var connection = Database.GetConnection();
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
        SELECT s.Name, s.Email
        FROM Enrollments e
        JOIN Students s ON e.StudentId = s.Id
        WHERE e.CourseId = @CourseId;
    ";
            command.Parameters.AddWithValue("@CourseId", courseId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                students.Add((reader.GetString(0), reader.GetString(1)));
            }

            return students;
        }
        
        

    }
}
