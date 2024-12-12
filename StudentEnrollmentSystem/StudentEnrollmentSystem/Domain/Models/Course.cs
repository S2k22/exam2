namespace Domain.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty; // Ensure non-null default
        public int Credits { get; set; }
    }
}