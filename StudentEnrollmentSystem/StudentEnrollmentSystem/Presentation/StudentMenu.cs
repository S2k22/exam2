using System;
using Domain.Services;

namespace Presentation
{
    public static class StudentMenu
    {
        private static readonly StudentService StudentService = new();

        public static void Show()
        {
            while (true)
            {
                ShowHeader("Manage Students");

                Console.WriteLine("1. Add Student");
                Console.WriteLine("2. List Students");
                Console.WriteLine("3. Edit Student");
                Console.WriteLine("4. Delete Student");
                Console.WriteLine("5. Search Students");
                Console.WriteLine("6. Show Student Performance Report");
                Console.WriteLine("7. Back to Main Menu");

                Console.WriteLine("========================================");
                Console.Write("Enter your choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddStudent();
                        break;
                    case "2":
                        ListStudents();
                        break;
                    case "3":
                        EditStudent();
                        break;
                    case "4":
                        DeleteStudent();
                        break;
                    case "5":
                        SearchStudents();
                        break;
                    case "6":
                        ShowStudentPerformanceReport();
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

        private static void AddStudent()
        {
            Console.Write("Enter Student Name: ");
            var name = Console.ReadLine();
            Console.Write("Enter Student Email: ");
            var email = Console.ReadLine();

            try
            {
                StudentService.AddStudent(name, email);
                ShowMessage("Student added successfully!", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                ShowMessage($"Error: {ex.Message}", ConsoleColor.Red);
            }
            Console.ReadKey();
        }

        private static void ListStudents()
        {
            Console.Clear();
            StudentService.ListStudents();
            Console.WriteLine("Press any key to go back...");
            Console.ReadKey();
        }

        private static void EditStudent()
        {
            Console.Write("Enter Student ID to edit: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                ShowMessage("Invalid Student ID. Press any key to try again...", ConsoleColor.Red);
                Console.ReadKey();
                return;
            }

            Console.Write("Enter New Name: ");
            var newName = Console.ReadLine();
            Console.Write("Enter New Email: ");
            var newEmail = Console.ReadLine();

            try
            {
                StudentService.EditStudent(studentId, newName, newEmail);
                ShowMessage("Student details updated successfully!", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                ShowMessage($"Error: {ex.Message}", ConsoleColor.Red);
            }
            Console.ReadKey();
        }

        private static void DeleteStudent()
        {
            Console.Write("Enter Student ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int studentId))
            {
                try
                {
                    StudentService.DeleteStudent(studentId);
                    ShowMessage("Student deleted successfully!", ConsoleColor.Green);
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

        private static void SearchStudents()
        {
            Console.Write("Enter name or email to search: ");
            var query = Console.ReadLine();
            StudentService.SearchStudents(query);
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
        
        private static void ShowStudentPerformanceReport()
        {
            Console.Write("Enter Student ID to view performance report: ");
            if (int.TryParse(Console.ReadLine(), out int studentId))
            {
                StudentService.ShowStudentPerformance(studentId);
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Invalid Student ID. Press any key to try again...");
                Console.ReadKey();
            }
        }

    }
}
