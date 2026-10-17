using System.ComponentModel.DataAnnotations;

namespace UniversityApi.Models;

public class Teacher
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(160)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string Department { get; set; } = string.Empty;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
