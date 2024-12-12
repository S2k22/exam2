using System;
using System.Threading.Tasks;
using Domain.Services;
using Data;
using Presentation;

namespace StudentEnrollmentSystem
{
    public static class MainMenu
    {
        public static async Task ShowAsync()
        {
            // Initialize dependencies
            var courseRepository = new CourseRepository();
            var courseService = new CourseService(courseRepository);
            var courseMenu = new CourseMenu(courseService);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- Student Enrollment System ---");
                Console.WriteLine("1. Manage Courses");
                Console.WriteLine("2. Exit");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await courseMenu.ShowMenuAsync();
                        break;
                    case "2":
                        Console.WriteLine("Exiting the system...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to try again...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
