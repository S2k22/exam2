using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Data;

namespace Domain.Services
{
    public class CourseService
    {
        private readonly CourseRepository _repository;

        // Inject the repository dependency
        public CourseService(CourseRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task AddCourseAsync(string title, int credits)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title), "Course title cannot be null or empty.");

            if (credits <= 0)
                throw new ArgumentException("Credits must be a positive number.");

            var course = new Course
            {
                Title = title,
                Credits = credits
            };

            await _repository.AddAsync(course);
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Course ID must be greater than zero.");

            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<Course>> SearchCoursesAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException(nameof(query), "Search query cannot be null or empty.");

            return await _repository.SearchAsync(query);
        }

        public async Task UpdateCourseAsync(int id, string title, int credits)
        {
            if (id <= 0)
                throw new ArgumentException("Course ID must be greater than zero.");
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title), "Course title cannot be null or empty.");
            if (credits <= 0)
                throw new ArgumentException("Credits must be a positive number.");

            var course = new Course
            {
                Id = id,
                Title = title,
                Credits = credits
            };

            await _repository.UpdateAsync(course);
        }

        public async Task DeleteCourseAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Course ID must be greater than zero.");

            await _repository.DeleteAsync(id);
        }

        public async Task<bool> IsTitleTakenAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title), "Course title cannot be null or empty.");

            return await _repository.TitleExistsAsync(title);
        }

        public async Task<List<(string Title, int EnrollmentCount)>> GetPopularCoursesAsync(int minEnrollment)
        {
            if (minEnrollment <= 0)
                throw new ArgumentException("Minimum enrollment must be greater than zero.");

            return await _repository.GetPopularCoursesAsync(minEnrollment);
        }
    }
}
