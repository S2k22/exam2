using System;
using System.Threading.Tasks;
using Domain.Services;
using Data;
using Presentation;
using StudentEnrollmentSystem.Domain.Services;

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
            var studentService = new StudentService();
            var enrollmentService = new EnrollmentService();
            var dashboardService = new DashboardService(studentService, enrollmentService);

            while (true)
            {
                ShowHeader("Student Enrollment System");
                Console.WriteLine("1. Manage Courses");
                Console.WriteLine("2. Manage Students");
                Console.WriteLine("3. Manage Enrollments");
                Console.WriteLine("4. View Dashboard");
                Console.WriteLine("5. Help");
                Console.WriteLine("6. Exit");
                Console.WriteLine("========================================");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await courseMenu.ShowMenuAsync();
                        break;
                    case "2":
                        NavigateTo("Manage Students");
                        StudentMenu.Show();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Press any key to try again...");
                        Console.ReadKey();
                        break;
                    case "3":
                        NavigateTo("Manage Enrollments");
                        EnrollmentMenu.Show();
                        break;
                    case "4":

                        dashboardService.ShowDashboard();

                        break;
                    case "5":
                        ShowHelp();
                        break;
                    case "6":
                        ShowMessage("Exiting... Goodbye!", ConsoleColor.Green);
                        return;
                }
            }
        }
        private static void ShowHeader(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=', 40));
            Console.WriteLine($"         {title.ToUpper()}         ");
            Console.WriteLine(new string('=', 40));
            Console.ResetColor();
        }

        private static void NavigateTo(string menuName)
        {
            Console.Clear();
            ShowMessage($"Navigating to {menuName}...", ConsoleColor.Gray);
            Thread.Sleep(1000);
        }

        private static void ShowMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }


        private static void ShowHelp()
        {
            Console.Clear();
            ShowHeader("Help");
            Console.WriteLine("Here are the features of the Student Enrollment System:");
            Console.WriteLine("1. Manage Students: Add, list, search, edit, or delete student records.");
            Console.WriteLine("2. Manage Courses: Add, list, search, edit, or delete course records.");
            Console.WriteLine("3. Manage Enrollments: Enroll students in courses, list enrollments, or delete enrollments.");
            Console.WriteLine("4. View Dashboard: Displays an overview of the system, including total counts.");
            Console.WriteLine("5. Help: Displays this help menu.");
            Console.WriteLine("6. Exit: Exits the system.");
            Console.WriteLine("========================================");
            Console.WriteLine("Press any key to go back to the main menu...");
            Console.ReadKey();
        }
    }
}
