using AutoMapper;
using UniversityApi.DTO.Students;
using UniversityApi.DTO.Teachers;
using UniversityApi.DTO.Courses;
using UniversityApi.DTO.Enrollments;
using UniversityApi.Models;

namespace UniversityApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Student, StudentDto>()
            .ForMember(d => d.CourseIds, o => o.MapFrom(s => s.Enrollments.Select(e => e.CourseId)));
        CreateMap<StudentCreateDto, Student>();
        CreateMap<StudentUpdateDto, Student>();

        CreateMap<Teacher, TeacherDto>();
        CreateMap<TeacherCreateDto, Teacher>();
        CreateMap<TeacherUpdateDto, Teacher>();

        CreateMap<Course, CourseDto>()
            .ForMember(d => d.TeacherName, o => o.MapFrom(s => s.Teacher == null ? null : s.Teacher.FirstName + " " + s.Teacher.LastName));
        CreateMap<CourseCreateDto, Course>();
        CreateMap<CourseUpdateDto, Course>();

        CreateMap<Enrollment, EnrollmentDto>()
            .ForMember(d => d.StudentName, o => o.MapFrom(e => e.Student == null ? null : e.Student.FirstName + " " + e.Student.LastName))
            .ForMember(d => d.CourseName, o => o.MapFrom(e => e.Course == null ? null : e.Course.Name));
    }
}
