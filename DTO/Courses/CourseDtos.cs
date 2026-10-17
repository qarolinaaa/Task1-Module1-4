using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO.Courses;

public class CourseCreateDto
{
    [Required, StringLength(120, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;
    [Range(1, 30)]
    public int Credits { get; set; }
    [Range(1, int.MaxValue)]
    public int TeacherId { get; set; }
}
public class CourseUpdateDto : CourseCreateDto { }
public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int TeacherId { get; set; }
    public string? TeacherName { get; set; }
    public DateTime CreatedAt { get; set; }
}
