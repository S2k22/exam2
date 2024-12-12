namespace Domain.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Ensure non-null default
        public string Email { get; set; } = string.Empty; // Ensure non-null default
    }
}