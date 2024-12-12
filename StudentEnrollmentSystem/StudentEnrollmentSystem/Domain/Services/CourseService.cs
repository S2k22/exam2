using Domain.Models;
using Data;
using System;

namespace Domain.Services
{
    public class CourseService
    {
        private readonly CourseRepository _repository = new();

        public void AddCourse(string title, int credits)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title), "Course title cannot be null or empty.");
            if (credits <= 0)
                throw new ArgumentException("Credits must be a positive number.", nameof(credits));
            if (_repository.TitleExists(title))
                throw new InvalidOperationException($"A course with the title '{title}' already exists.");

            var course = new Course { Title = title, Credits = credits };
            _repository.Add(course);
        }


        public void ListCourses(int pageSize = 5)
        {
            var courses = _repository.GetAll();
            int totalPages = (int)Math.Ceiling(courses.Count / (double)pageSize);

            for (int currentPage = 1; currentPage <= totalPages; currentPage++)
            {
                Console.Clear();
                Console.WriteLine($"=== List of Courses (Page {currentPage}/{totalPages}) ===");

                var pageCourses = courses.Skip((currentPage - 1) * pageSize).Take(pageSize);
                foreach (var course in pageCourses)
                {
                    Console.WriteLine($"Course ID: {course.Id}, Title: {course.Title}, Credits: {course.Credits}");
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

        public void SearchCourses(string query)
        {
            var courses = _repository.Search(query);
            if (courses.Count == 0)
            {
                Console.WriteLine($"No courses found matching '{query}'.");
            }
            else
            {
                Console.WriteLine("=== Search Results ===");
                foreach (var course in courses)
                {
                    Console.WriteLine($"Course ID: {course.Id}, Title: {course.Title}, Credits: {course.Credits}");
                }
            }
        }

        public void DeleteCourse(int courseId)
        {
            var course = _repository.GetById(courseId);
            if (course == null)
            {
                Console.WriteLine($"No course found with ID {courseId}.");
                return;
            }

            Console.Write($"Are you sure you want to delete Course '{course.Title}' with ID {courseId}? (y/n): ");
            var confirmation = Console.ReadLine()?.ToLower();

            if (confirmation == "y")
            {
                try
                {
                    _repository.Delete(courseId);
                    Console.WriteLine($"Course '{course.Title}' with ID {courseId} deleted successfully.");
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
        
        public void EditCourse(int courseId, string newTitle, int newCredits)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Course title cannot be empty.", nameof(newTitle));
            if (newCredits <= 0)
                throw new ArgumentException("Credits must be a positive number.", nameof(newCredits));

            var course = _repository.GetById(courseId);
            if (course == null)
                throw new InvalidOperationException($"No course found with ID {courseId}.");

            course.Title = newTitle;
            course.Credits = newCredits;

            _repository.Update(course);
        }
        
        public List<Course> GetAllCourses()
        {
            return _repository.GetAll(); // Fetches all courses from the repository
        }

        public void ShowPopularCourses()
        {
            var popularCourses = _repository.GetPopularCourses();

            Console.WriteLine("=== Popular Courses ===");
            foreach (var (courseTitle, enrollmentCount) in popularCourses)
            {
                Console.WriteLine($"- {courseTitle}: {enrollmentCount} enrollments");
            }
        }


    }
}