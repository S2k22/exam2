namespace Domain.Models
{
    public class Course
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty; // Ensure non-null default
        public int Credits { get; init; }
        public string? Description { get; set; } // Optional property

        public override string ToString() =>
            $"{Title} (Credits: {Credits})";

        public override bool Equals(object? obj)
        {
            if (obj is not Course other) return false;
            return Id == other.Id && Title == other.Title && Credits == other.Credits;
        }

        public override int GetHashCode() => HashCode.Combine(Id, Title, Credits);

        public static bool operator ==(Course? c1, Course? c2) =>
            c1 is null ? c2 is null : c1.Equals(c2);

        public static bool operator !=(Course? c1, Course? c2) => !(c1 == c2);
    }
}
