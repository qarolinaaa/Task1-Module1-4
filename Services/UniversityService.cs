using UniversityApi.Models;
using UniversityApi.Repositories;

namespace UniversityApi.Services;

public interface IUniversityService
{
    Task<bool> TeacherExistsAsync(int id);
    Task<bool> StudentExistsAsync(int id);
    Task<bool> CourseExistsAsync(int id);
}
public class UniversityService : IUniversityService
{
    private readonly ITeacherRepository _teachers;
    private readonly IStudentRepository _students;
    private readonly ICourseRepository _courses;
    public UniversityService(ITeacherRepository teachers, IStudentRepository students, ICourseRepository courses)
    { _teachers = teachers; _students = students; _courses = courses; }
    public async Task<bool> TeacherExistsAsync(int id) => await _teachers.GetByIdAsync(id) is not null;
    public async Task<bool> StudentExistsAsync(int id) => await _students.GetByIdAsync(id) is not null;
    public async Task<bool> CourseExistsAsync(int id) => await _courses.GetByIdAsync(id) is not null;
}
