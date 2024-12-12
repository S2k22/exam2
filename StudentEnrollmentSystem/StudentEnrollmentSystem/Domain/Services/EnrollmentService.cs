using Domain.Models;
using Data;
using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public class EnrollmentService
    {
        private readonly EnrollmentRepository _repository = new();

        // Predefined set of valid grades
        private static readonly HashSet<string> ValidGrades = new() { "A", "B", "C", "D", "F", "Pass", "Fail" };

        public void EnrollStudent(int studentId, int courseId, string? grade = null)
        {
            if (studentId <= 0)
                throw new ArgumentException("Invalid Student ID.", nameof(studentId));
            if (courseId <= 0)
                throw new ArgumentException("Invalid Course ID.", nameof(courseId));
            if (_repository.EnrollmentExists(studentId, courseId))
                throw new InvalidOperationException("This student is already enrolled in the selected course.");
            if (!string.IsNullOrWhiteSpace(grade) && !IsValidGrade(grade))
                throw new ArgumentException($"Invalid grade '{grade}'. Valid grades are: {string.Join(", ", ValidGrades)}", nameof(grade));

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                Grade = grade,
                EnrollmentDate = DateTime.Now // Automatically set the current date and time
            };
            _repository.Add(enrollment);
        }

        public void ListEnrollments()
        {
            var enrollments = _repository.GetAllWithDetails();
            Console.WriteLine("=== List of Enrollments ===");
            foreach (var (enrollment, studentName, courseTitle) in enrollments)
            {
                Console.WriteLine(
                    $"Enrollment ID: {enrollment.Id}, Student: {studentName}, Course: {courseTitle}, " +
                    $"Grade: {enrollment.Grade ?? "N/A"}, Date: {enrollment.EnrollmentDate:yyyy-MM-dd HH:mm:ss}"
                );
            }
        }

        public void DeleteEnrollment(int enrollmentId)
        {
            Console.Write($"Are you sure you want to delete Enrollment with ID {enrollmentId}? (y/n): ");
            var confirmation = Console.ReadLine()?.ToLower();

            if (confirmation == "y")
            {
                _repository.Delete(enrollmentId);
                Console.WriteLine($"Enrollment with ID {enrollmentId} deleted successfully.");
            }
            else
            {
                Console.WriteLine("Delete operation canceled.");
            }
        }

        private bool IsValidGrade(string grade)
        {
            // Check if the provided grade exists in the predefined set of valid grades
            return ValidGrades.Contains(grade);
        }
        
        public List<Enrollment> GetAllEnrollments()
        {
            return _repository.GetAllWithDetails()
                .Select(e => e.Item1) // Only return Enrollment objects
                .ToList();
        }
        
        public void ShowCourseEnrollment(int courseId)
        {
            var courseDetails = _repository.GetEnrollmentsByCourse(courseId);

            if (courseDetails.Count == 0)
            {
                Console.WriteLine($"No students are enrolled in Course ID {courseId}.");
            }
            else
            {
                Console.WriteLine($"=== Students Enrolled in Course ID {courseId} ===");
                foreach (var (studentName, studentEmail) in courseDetails)
                {
                    Console.WriteLine($"- {studentName} ({studentEmail})");
                }
            }
        }
        
        

    }
}
