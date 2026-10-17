using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApi.DTO.Enrollments;
using UniversityApi.Models;
using UniversityApi.Repositories;
using UniversityApi.Services;

namespace UniversityApi.Controllers;

[Route("api/[controller]")]
public class EnrollmentsController : ApiControllerBase
{
    private readonly IEnrollmentRepository _repo;
    private readonly IUniversityService _service;
    private readonly IMapper _mapper;
    private readonly ILogger<EnrollmentsController> _logger;
    public EnrollmentsController(IEnrollmentRepository repo, IUniversityService service, IMapper mapper, ILogger<EnrollmentsController> logger)
    { _repo = repo; _service = service; _mapper = mapper; _logger = logger; }

    [HttpGet]
    public async Task<IActionResult> GetAll() => OkResult(_mapper.Map<List<EnrollmentDto>>(await _repo.GetDetailedAsync()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _repo.GetDetailedByIdAsync(id);
        if (entity is null) { _logger.LogWarning("Enrollment {Id} not found", id); return Error<EnrollmentDto>(404, "ENROLLMENT_NOT_FOUND", "Enrollment not found."); }
        return OkResult(_mapper.Map<EnrollmentDto>(entity));
    }

    [HttpPost]
    public async Task<IActionResult> Create(EnrollmentCreateDto dto)
    {
        if (!await _service.StudentExistsAsync(dto.StudentId)) return Error<EnrollmentDto>(400, "STUDENT_NOT_FOUND", "The specified student does not exist.");
        if (!await _service.CourseExistsAsync(dto.CourseId)) return Error<EnrollmentDto>(400, "COURSE_NOT_FOUND", "The specified course does not exist.");
        if (await _repo.ExistsAsync(dto.StudentId, dto.CourseId)) return Error<EnrollmentDto>(409, "DUPLICATE_ENROLLMENT", "This student is already enrolled in this course.");
        var entity = new Enrollment { StudentId = dto.StudentId, CourseId = dto.CourseId, EnrollmentDate = DateTime.UtcNow };
        var created = await _repo.AddAsync(entity);
        var detailed = await _repo.GetDetailedByIdAsync(created.Id);
        _logger.LogInformation("Created enrollment {Id}", created.Id);
        return StatusCode(201, UniversityApi.Common.ReturnResult<EnrollmentDto>.Success(_mapper.Map<EnrollmentDto>(detailed), 201, HttpContext.TraceIdentifier));
    }

    [HttpPut("{id:int}/grade")]
    public async Task<IActionResult> UpdateGrade(int id, EnrollmentGradeUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) { _logger.LogWarning("Grade update requested for missing enrollment {Id}", id); return Error<EnrollmentDto>(404, "ENROLLMENT_NOT_FOUND", "Enrollment not found."); }
        entity.Grade = dto.Grade;
        await _repo.UpdateAsync(entity);
        _logger.LogInformation("Updated grade for enrollment {Id}", id);
        return OkResult(_mapper.Map<EnrollmentDto>(await _repo.GetDetailedByIdAsync(id)));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) { _logger.LogWarning("Delete requested for missing enrollment {Id}", id); return Error<object>(404, "ENROLLMENT_NOT_FOUND", "Enrollment not found."); }
        await _repo.DeleteAsync(entity); _logger.LogInformation("Deleted enrollment {Id}", id);
        return OkResult(new { message = "Enrollment deleted." });
    }
}
