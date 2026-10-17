using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO.Teachers;

public class TeacherCreateDto
{
    [Required, StringLength(80, MinimumLength = 1)]
    public string FirstName { get; set; } = string.Empty;
    [Required, StringLength(80, MinimumLength = 1)]
    public string LastName { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(160)]
    public string Email { get; set; } = string.Empty;
    [Required, StringLength(120, MinimumLength = 1)]
    public string Department { get; set; } = string.Empty;
}
public class TeacherUpdateDto : TeacherCreateDto { }
public class TeacherDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}
