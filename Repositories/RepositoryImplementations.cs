using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Models;

namespace UniversityApi.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext Db;
    protected readonly DbSet<T> Set;
    protected Repository(ApplicationDbContext db) { Db = db; Set = db.Set<T>(); }
    public virtual Task<List<T>> GetAllAsync() => Set.AsNoTracking().ToListAsync();
    public virtual Task<T?> GetByIdAsync(int id) => Set.FindAsync(id).AsTask();
    public virtual async Task<T> AddAsync(T entity) { Set.Add(entity); await Db.SaveChangesAsync(); return entity; }
    public virtual async Task UpdateAsync(T entity) { Db.Entry(entity).State = EntityState.Modified; await Db.SaveChangesAsync(); }
    public virtual async Task DeleteAsync(T entity) { Set.Remove(entity); await Db.SaveChangesAsync(); }
}
public class StudentRepository : Repository<Student>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext db) : base(db) { }
    public Task<Student?> GetWithCoursesAsync(int id) => Db.Students.Include(s => s.Enrollments).ThenInclude(e => e.Course).FirstOrDefaultAsync(s => s.Id == id);
}
public class TeacherRepository : Repository<Teacher>, ITeacherRepository
{
    public TeacherRepository(ApplicationDbContext db) : base(db) { }
}
public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext db) : base(db) { }
    public Task<List<Course>> FilterAsync(string? name, int? teacherId, int? minCredits)
    {
        var query = Db.Courses.AsNoTracking().Include(c => c.Teacher).AsQueryable();
        if (!string.IsNullOrWhiteSpace(name)) query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()));
        if (teacherId.HasValue) query = query.Where(c => c.TeacherId == teacherId.Value);
        if (minCredits.HasValue) query = query.Where(c => c.Credits >= minCredits.Value);
        return query.ToListAsync();
    }
}
public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(ApplicationDbContext db) : base(db) { }
    public Task<bool> ExistsAsync(int studentId, int courseId) => Db.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
    public Task<List<Enrollment>> GetDetailedAsync() => Db.Enrollments.AsNoTracking().Include(e => e.Student).Include(e => e.Course).ToListAsync();
    public Task<Enrollment?> GetDetailedByIdAsync(int id) => Db.Enrollments.Include(e => e.Student).Include(e => e.Course).FirstOrDefaultAsync(e => e.Id == id);
}
