using System;
using Domain.Services;

namespace Presentation
{
    public static class CourseMenu
    {
        private static readonly CourseService CourseService = new();

        public static void Show()
        {
            while (true)
            {
                ShowHeader("Manage Courses");

                Console.WriteLine("1. Add Course");
                Console.WriteLine("2. List Courses");
                Console.WriteLine("3. Edit Course");
                Console.WriteLine("4. Delete Course");
                Console.WriteLine("5. Search Courses");
                Console.WriteLine("6. Show Popular Courses");
                Console.WriteLine("7. Back to Main Menu");
                Console.WriteLine("========================================");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCourse();
                        break;
                    case "2":
                        ListCourses();
                        break;
                    case "3":
                        EditCourse();
                        break;
                    case "4":
                        DeleteCourse();
                        break;
                    case "5":
                        SearchCourses();
                        break;
                    case "6":
                        ShowPopularCoursesReport();
                        break;
                    case "7":
                        return;
                    default:
                        ShowMessage("Invalid choice. Press any key to try again...", ConsoleColor.Red);
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void AddCourse()
        {
            Console.Write("Enter Course Title: ");
            var title = Console.ReadLine();
            Console.Write("Enter Course Credits: ");
            if (!int.TryParse(Console.ReadLine(), out int credits))
            {
                ShowMessage("Invalid number of credits. Press any key to try again...", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            try
            {
                CourseService.AddCourse(title, credits);
                ShowMessage("Course added successfully!", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                ShowMessage($"Error: {ex.Message}", ConsoleColor.Red);
            }
            Console.ReadKey();
        }

        private static void ListCourses()
        {
            Console.Clear();
            CourseService.ListCourses();
            Console.WriteLine("Press any key to go back...");
            Console.ReadKey();
        }

        private static void EditCourse()
        {
            Console.Write("Enter Course ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                ShowMessage("Invalid Course ID. Press any key to try again...", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            Console.Write("Enter New Title: ");
            var newTitle = Console.ReadLine();
            Console.Write("Enter New Credits: ");
            if (!int.TryParse(Console.ReadLine(), out int newCredits))
            {
                ShowMessage("Invalid number of credits. Press any key to try again...", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            try
            {
                CourseService.EditCourse(courseId, newTitle, newCredits);
                ShowMessage("Course details updated successfully!", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                ShowMessage($"Error: {ex.Message}", ConsoleColor.Red);
            }
            Console.ReadKey();
        }

        private static void DeleteCourse()
        {
            Console.Write("Enter Course ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int courseId))
            {
                try
                {
                    CourseService.DeleteCourse(courseId);
                    ShowMessage("Course deleted successfully!", ConsoleColor.Green);
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

        private static void SearchCourses()
        {
            Console.Write("Enter course title to search: ");
            var query = Console.ReadLine();
            CourseService.SearchCourses(query);
            Console.WriteLine("Press any key to continue...");
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
        
        private static void ShowPopularCoursesReport()
        {
            CourseService.ShowPopularCourses();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

    }
}
