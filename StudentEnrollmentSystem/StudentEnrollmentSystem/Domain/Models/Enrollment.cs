namespace Domain.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public string? Grade { get; set; } // Allow null for Grade
        public DateTime EnrollmentDate { get; set; } // New property for enrollment date
    }
}