using System;
using Domain.Services;

namespace Presentation
{
    public static class EnrollmentMenu
    {
        private static readonly EnrollmentService EnrollmentService = new();

        public static void Show()
        {
            while (true)
            {
                ShowHeader("Manage Enrollments");

                Console.WriteLine("1. Enroll Student in Course");
                Console.WriteLine("2. List Enrollments");
                Console.WriteLine("3. Delete Enrollment");
                Console.WriteLine("4. Show Course Enrollment Report");
                Console.WriteLine("5. Back to Main Menu");
                Console.WriteLine("========================================");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        EnrollStudent();
                        break;
                    case "2":
                        ListEnrollments();
                        break;
                    case "3":
                        DeleteEnrollment();
                        break;
                    case "4":
                        ShowCourseEnrollmentReport();
                        break;
                    case "5":
                        return;

                    default:
                        ShowMessage("Invalid choice. Press any key to try again...", ConsoleColor.Red);
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void EnrollStudent()
        {
            Console.Write("Enter Student ID: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                ShowMessage("Invalid Student ID. Press any key to try again...", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Course ID: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                ShowMessage("Invalid Course ID. Press any key to try again...", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            Console.Write("Enter Grade (optional, valid grades: A, B, C, D, F, Pass, Fail): ");
            var grade = Console.ReadLine();

            try
            {
                EnrollmentService.EnrollStudent(studentId, courseId, grade);
                ShowMessage("Student enrolled successfully!", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                ShowMessage($"Error: {ex.Message}", ConsoleColor.Red);
            }
            Console.ReadKey();
        }

        private static void ListEnrollments()
        {
            Console.Clear();
            EnrollmentService.ListEnrollments();
            Console.WriteLine("Press any key to go back...");
            Console.ReadKey();
        }

        private static void DeleteEnrollment()
        {
            Console.Write("Enter Enrollment ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int enrollmentId))
            {
                try
                {
                    EnrollmentService.DeleteEnrollment(enrollmentId);
                    ShowMessage("Enrollment deleted successfully!", ConsoleColor.Green);
                }
                catch (Exception ex)
                {
                    ShowMessage($"Error: {ex.Message}", ConsoleColor.Red);
                }
            }
            else
            {
                ShowMessage("Invalid ID. Press any key to try again...", ConsoleColor.Red);
            }
            Console.ReadKey();
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

        private static void ShowMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        
        private static void ShowCourseEnrollmentReport()
        {
            Console.Write("Enter Course ID to view enrollment report: ");
            if (int.TryParse(Console.ReadLine(), out int courseId))
            {
                EnrollmentService.ShowCourseEnrollment(courseId);
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Invalid Course ID. Press any key to try again...");
                Console.ReadKey();
            }
        }

    }
}
