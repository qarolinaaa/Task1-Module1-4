using UniversityApi.Models;

namespace UniversityApi.Repositories;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
public interface IStudentRepository : IRepository<Student> { Task<Student?> GetWithCoursesAsync(int id); }
public interface ITeacherRepository : IRepository<Teacher> { }
public interface ICourseRepository : IRepository<Course> { Task<List<Course>> FilterAsync(string? name, int? teacherId, int? minCredits); }
public interface IEnrollmentRepository : IRepository<Enrollment>
{
    Task<bool> ExistsAsync(int studentId, int courseId);
    Task<List<Enrollment>> GetDetailedAsync();
    Task<Enrollment?> GetDetailedByIdAsync(int id);
}
