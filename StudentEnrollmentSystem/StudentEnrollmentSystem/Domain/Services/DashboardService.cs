
using System;
using Domain.Services;
using StudentEnrollmentSystem.Domain.Services;

namespace StudentEnrollmentSystem.Domain.Services
{
    public class DashboardService
    {
        private readonly StudentService _studentService;
        private readonly EnrollmentService _enrollmentService;

        public DashboardService(StudentService studentService, EnrollmentService enrollmentService)
        {
            _studentService = studentService;
            _enrollmentService = enrollmentService;
        }

        public void ShowDashboard()
        {
            Console.Clear();
            Console.WriteLine("Dashboard");
            Console.WriteLine("========================================");

            var totalStudents = _studentService.GetAllStudents().Count;
            var totalEnrollments = _enrollmentService.GetAllEnrollments().Count;

            Console.WriteLine($"Total Students Registered: {totalStudents}");
            Console.WriteLine($"Total Enrollments: {totalEnrollments}");
            Console.WriteLine("========================================");
            Console.WriteLine("Press any key to go back to the main menu...");
            Console.ReadKey();
        }
    }
}
