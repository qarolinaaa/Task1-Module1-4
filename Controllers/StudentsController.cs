using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApi.DTO.Students;
using UniversityApi.Models;
using UniversityApi.Repositories;

namespace UniversityApi.Controllers;

[Route("api/[controller]")]
public class StudentsController : ApiControllerBase
{
    private readonly IStudentRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<StudentsController> _logger;
    public StudentsController(IStudentRepository repo, IMapper mapper, ILogger<StudentsController> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    [HttpGet]
    public async Task<IActionResult> GetAll() => OkResult(_mapper.Map<List<StudentDto>>(await _repo.GetAllAsync()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _repo.GetWithCoursesAsync(id);
        if (entity is null) { _logger.LogWarning("Student {Id} not found", id); return Error<StudentDto>(404, "STUDENT_NOT_FOUND", "Student not found."); }
        return OkResult(_mapper.Map<StudentDto>(entity));
    }

    [HttpPost]
    public async Task<IActionResult> Create(StudentCreateDto dto)
    {
        try
        {
            var entity = _mapper.Map<Student>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            var created = await _repo.AddAsync(entity);
            _logger.LogInformation("Created student {StudentId}", created.Id);
            return StatusCode(201, UniversityApi.Common.ReturnResult<StudentDto>.Success(_mapper.Map<StudentDto>(created), 201, HttpContext.TraceIdentifier));
        }
        catch (DbUpdateException) { return Error<StudentDto>(409, "STUDENT_CONFLICT", "A student with this email may already exist."); }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, StudentUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) { _logger.LogWarning("Update requested for missing student {Id}", id); return Error<StudentDto>(404, "STUDENT_NOT_FOUND", "Student not found."); }
        _mapper.Map(dto, entity);
        try { await _repo.UpdateAsync(entity); _logger.LogInformation("Updated student {StudentId}", id); return OkResult(_mapper.Map<StudentDto>(entity)); }
        catch (DbUpdateException) { return Error<StudentDto>(409, "STUDENT_CONFLICT", "A student with this email may already exist."); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) { _logger.LogWarning("Delete requested for missing student {Id}", id); return Error<object>(404, "STUDENT_NOT_FOUND", "Student not found."); }
        await _repo.DeleteAsync(entity); _logger.LogInformation("Deleted student {StudentId}", id);
        return OkResult(new { message = "Student deleted." });
    }
}
