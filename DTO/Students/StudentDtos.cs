using System.ComponentModel.DataAnnotations;

namespace UniversityApi.DTO.Students;

public class StudentCreateDto
{
    [Required, StringLength(80, MinimumLength = 1)]
    public string FirstName { get; set; } = string.Empty;
    [Required, StringLength(80, MinimumLength = 1)]
    public string LastName { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(160)]
    public string Email { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
}

public class StudentUpdateDto : StudentCreateDto { }

public class StudentDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<int> CourseIds { get; set; } = new();
}
