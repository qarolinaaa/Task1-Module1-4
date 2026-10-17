using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO.Enrollments;

public class EnrollmentCreateDto
{
    [Range(1, int.MaxValue)]
    public int StudentId { get; set; }
    [Range(1, int.MaxValue)]
    public int CourseId { get; set; }
}
public class EnrollmentGradeUpdateDto
{
    [Range(0, 100)]
    public decimal? Grade { get; set; }
}
public class EnrollmentDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public int CourseId { get; set; }
    public string? CourseName { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public decimal? Grade { get; set; }
}
