using Domain.Models;
using Data;
using System;
using System.Text.RegularExpressions;

namespace Domain.Services
{
    public class StudentService
    {
        private readonly StudentRepository _repository = new();

        public void AddStudent(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Student name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email), "Student email cannot be null or empty.");
            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format.", nameof(email));
            if (_repository.EmailExists(email))
                throw new InvalidOperationException($"A student with the email '{email}' already exists.");

            var student = new Student { Name = name, Email = email };
            _repository.Add(student);
        }


        public void ListStudents(int pageSize = 5)
        {
            var students = _repository.GetAll();
            int totalPages = (int)Math.Ceiling(students.Count / (double)pageSize);

            for (int currentPage = 1; currentPage <= totalPages; currentPage++)
            {
                Console.Clear();
                Console.WriteLine($"=== List of Students (Page {currentPage}/{totalPages}) ===");

                var pageStudents = students.Skip((currentPage - 1) * pageSize).Take(pageSize);
                foreach (var student in pageStudents)
                {
                    Console.WriteLine($"Student ID: {student.Id}, Name: {student.Name}, Email: {student.Email}");
                }

                if (currentPage < totalPages)
                {
                    Console.WriteLine("Press any key for next page...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("End of list. Press any key to continue...");
            Console.ReadKey();
        }

        public void SearchStudents(string query)
        {
            var students = _repository.Search(query);
            if (students.Count == 0)
            {
                Console.WriteLine($"No students found matching '{query}'.");
            }
            else
            {
                Console.WriteLine("=== Search Results ===");
                foreach (var student in students)
                {
                    Console.WriteLine($"Student ID: {student.Id}, Name: {student.Name}, Email: {student.Email}");
                }
            }
        }

        public void DeleteStudent(int studentId)
        {
            var student = _repository.GetById(studentId);
            if (student == null)
            {
                Console.WriteLine($"No student found with ID {studentId}.");
                return;
            }

            Console.Write($"Are you sure you want to delete Student '{student.Name}' with ID {studentId}? (y/n): ");
            var confirmation = Console.ReadLine()?.ToLower();

            if (confirmation == "y")
            {
                try
                {
                    _repository.Delete(studentId);
                    Console.WriteLine($"Student '{student.Name}' with ID {studentId} deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Delete operation canceled.");
            }
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Regex pattern for validating email addresses
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
        
        public void EditStudent(int studentId, string newName, string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("Student name cannot be empty.", nameof(newName));
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentException("Student email cannot be empty.", nameof(newEmail));
            if (!IsValidEmail(newEmail))
                throw new ArgumentException("Invalid email format.", nameof(newEmail));

            var student = _repository.GetById(studentId);
            if (student == null)
                throw new InvalidOperationException($"No student found with ID {studentId}.");

            student.Name = newName;
            student.Email = newEmail;

            _repository.Update(student);
        }
        
        public List<Student> GetAllStudents()
        {
            return _repository.GetAll(); // Fetches all students from the repository
        }
        
        public void ShowStudentPerformance(int studentId)
        {
            var performance = _repository.GetStudentPerformance(studentId);

            if (performance.Count == 0)
            {
                Console.WriteLine($"Student ID {studentId} is not enrolled in any courses.");
            }
            else
            {
                Console.WriteLine($"=== Performance for Student ID {studentId} ===");
                foreach (var (courseTitle, grade) in performance)
                {
                    Console.WriteLine($"- {courseTitle}: Grade: {grade ?? "N/A"}");
                }
            }
        }


    }
    
}
