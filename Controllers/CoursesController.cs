using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApi.DTO.Courses;
using UniversityApi.Models;
using UniversityApi.Repositories;
using UniversityApi.Services;

namespace UniversityApi.Controllers;

[Route("api/[controller]")]
public class CoursesController : ApiControllerBase
{
    private readonly ICourseRepository _repo;
    private readonly IUniversityService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<CoursesController> _logger;
    public CoursesController(ICourseRepository repo, IUniversityService service, IMapper mapper, ILogger<CoursesController> logger)
    { _repo = repo; _service = service; _mapper = mapper; _logger = logger; }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] int? teacherId, [FromQuery] int? minCredits)
    {
        var courses = string.IsNullOrWhiteSpace(name) && !teacherId.HasValue && !minCredits.HasValue
            ? await _repo.GetAllAsync() : await _repo.FilterAsync(name, teacherId, minCredits);
        return OkResult(_mapper.Map<List<CourseDto>>(courses));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) { _logger.LogWarning("Course {Id} not found", id); return Error<CourseDto>(404, "COURSE_NOT_FOUND", "Course not found."); }
        return OkResult(_mapper.Map<CourseDto>(entity));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateDto dto)
    {
        if (!await _service.TeacherExistsAsync(dto.TeacherId)) return Error<CourseDto>(400, "TEACHER_NOT_FOUND", "The specified teacher does not exist.");
        var entity = _mapper.Map<Course>(dto); entity.CreatedAt = DateTime.UtcNow;
        var created = await _repo.AddAsync(entity); _logger.LogInformation("Created course {Id}", created.Id);
        return StatusCode(201, UniversityApi.Common.ReturnResult<CourseDto>.Success(_mapper.Map<CourseDto>(created), 201, HttpContext.TraceIdentifier));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CourseUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) { _logger.LogWarning("Update requested for missing course {Id}", id); return Error<CourseDto>(404, "COURSE_NOT_FOUND", "Course not found."); }
        if (!await _service.TeacherExistsAsync(dto.TeacherId)) return Error<CourseDto>(400, "TEACHER_NOT_FOUND", "The specified teacher does not exist.");
        _mapper.Map(dto, entity);
        try { await _repo.UpdateAsync(entity); _logger.LogInformation("Updated course {Id}", id); return OkResult(_mapper.Map<CourseDto>(entity)); }
        catch (DbUpdateException) { return Error<CourseDto>(409, "COURSE_CONFLICT", "Course could not be updated."); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) { _logger.LogWarning("Delete requested for missing course {Id}", id); return Error<object>(404, "COURSE_NOT_FOUND", "Course not found."); }
        await _repo.DeleteAsync(entity); _logger.LogInformation("Deleted course {Id}", id);
        return OkResult(new { message = "Course deleted." });
    }
}
