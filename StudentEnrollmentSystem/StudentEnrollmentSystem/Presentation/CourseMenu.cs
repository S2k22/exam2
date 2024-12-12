using System;
using System.Threading.Tasks;
using Domain.Services;
using Domain.Models;

namespace Presentation
{
    public class CourseMenu
    {
        private readonly CourseService _courseService;

        // Constructor for dependency injection
        public CourseMenu(CourseService courseService)
        {
            _courseService = courseService ?? throw new ArgumentNullException(nameof(courseService));
        }

        public async Task ShowMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Course Management ---");
                Console.WriteLine("1. Add Course");
                Console.WriteLine("2. List All Courses");
                Console.WriteLine("3. Edit Course");
                Console.WriteLine("4. Delete Course");
                Console.WriteLine("5. Search Courses");
                Console.WriteLine("6. Show Popular Courses");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await AddCourseAsync();
                        break;
                    case "2":
                        await ListCoursesAsync();
                        break;
                    case "3":
                        await EditCourseAsync();
                        break;
                    case "4":
                        await DeleteCourseAsync();
                        break;
                    case "5":
                        await SearchCoursesAsync();
                        break;
                    case "6":
                        await ShowPopularCoursesAsync();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task AddCourseAsync()
        {
            Console.Clear();
            Console.Write("Enter Course Title: ");
            var title = Console.ReadLine();

            Console.Write("Enter Number of Credits: ");
            if (!int.TryParse(Console.ReadLine(), out var credits))
            {
                Console.WriteLine("Invalid number of credits. Press any key to try again...");
                Console.ReadKey();
                return;
            }

            try
            {
                await _courseService.AddCourseAsync(title, credits);
                Console.WriteLine("Course added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task ListCoursesAsync()
        {
            Console.Clear();
            try
            {
                var courses = await _courseService.GetAllCoursesAsync();
                if (courses.Count == 0)
                {
                    Console.WriteLine("No courses found.");
                }
                else
                {
                    Console.WriteLine("--- List of Courses ---");
                    foreach (var course in courses)
                    {
                        Console.WriteLine($"{course.Id}: {course.Title} ({course.Credits} credits)");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task EditCourseAsync()
        {
            Console.Clear();
            Console.Write("Enter Course ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid ID. Press any key to try again...");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter New Course Title: ");
            var title = Console.ReadLine();

            Console.Write("Enter New Number of Credits: ");
            if (!int.TryParse(Console.ReadLine(), out var credits))
            {
                Console.WriteLine("Invalid number of credits. Press any key to try again...");
                Console.ReadKey();
                return;
            }

            try
            {
                await _courseService.UpdateCourseAsync(id, title, credits);
                Console.WriteLine("Course updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task DeleteCourseAsync()
        {
            Console.Clear();
            Console.Write("Enter Course ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid ID. Press any key to try again...");
                Console.ReadKey();
                return;
            }

            try
            {
                await _courseService.DeleteCourseAsync(id);
                Console.WriteLine("Course deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task SearchCoursesAsync()
        {
            Console.Clear();
            Console.Write("Enter search term: ");
            var query = Console.ReadLine();

            try
            {
                var courses = await _courseService.SearchCoursesAsync(query);
                if (courses.Count == 0)
                {
                    Console.WriteLine("No courses found.");
                }
                else
                {
                    Console.WriteLine("--- Search Results ---");
                    foreach (var course in courses)
                    {
                        Console.WriteLine($"{course.Id}: {course.Title} ({course.Credits} credits)");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private async Task ShowPopularCoursesAsync()
        {
            Console.Clear();
            Console.Write("Enter minimum enrollment: ");
            if (!int.TryParse(Console.ReadLine(), out var minEnrollment))
            {
                Console.WriteLine("Invalid number. Press any key to try again...");
                Console.ReadKey();
                return;
            }

            try
            {
                var popularCourses = await _courseService.GetPopularCoursesAsync(minEnrollment);
                if (popularCourses.Count == 0)
                {
                    Console.WriteLine("No popular courses found.");
                }
                else
                {
                    Console.WriteLine("--- Popular Courses ---");
                    foreach (var (title, enrollmentCount) in popularCourses)
                    {
                        Console.WriteLine($"{title}: {enrollmentCount} enrollments");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
