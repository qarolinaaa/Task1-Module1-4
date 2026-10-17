using System.ComponentModel.DataAnnotations;

namespace UniversityApi.Models;

public class Student
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(160)]
    public string Email { get; set; } = string.Empty;

    public DateTime BirthDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
