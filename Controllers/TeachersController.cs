using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityApi.DTO.Teachers;
using UniversityApi.Models;
using UniversityApi.Repositories;

namespace UniversityApi.Controllers;

[Route("api/[controller]")]
public class TeachersController : ApiControllerBase
{
    private readonly ITeacherRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<TeachersController> _logger;
    public TeachersController(ITeacherRepository repo, IMapper mapper, ILogger<TeachersController> logger)
    { _repo = repo; _mapper = mapper; _logger = logger; }

    [HttpGet]
    public async Task<IActionResult> GetAll() => OkResult(_mapper.Map<List<TeacherDto>>(await _repo.GetAllAsync()));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) { _logger.LogWarning("Teacher {Id} not found", id); return Error<TeacherDto>(404, "TEACHER_NOT_FOUND", "Teacher not found."); }
        return OkResult(_mapper.Map<TeacherDto>(entity));
    }

    [HttpPost]
    public async Task<IActionResult> Create(TeacherCreateDto dto)
    {
        try { var entity = _mapper.Map<Teacher>(dto); var created = await _repo.AddAsync(entity); _logger.LogInformation("Created teacher {Id}", created.Id); return StatusCode(201, UniversityApi.Common.ReturnResult<TeacherDto>.Success(_mapper.Map<TeacherDto>(created), 201, HttpContext.TraceIdentifier)); }
        catch (DbUpdateException) { return Error<TeacherDto>(409, "TEACHER_CONFLICT", "A teacher with this email may already exist."); }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TeacherUpdateDto dto)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) { _logger.LogWarning("Update requested for missing teacher {Id}", id); return Error<TeacherDto>(404, "TEACHER_NOT_FOUND", "Teacher not found."); }
        _mapper.Map(dto, entity);
        try { await _repo.UpdateAsync(entity); _logger.LogInformation("Updated teacher {Id}", id); return OkResult(_mapper.Map<TeacherDto>(entity)); }
        catch (DbUpdateException) { return Error<TeacherDto>(409, "TEACHER_CONFLICT", "A teacher with this email may already exist."); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) { _logger.LogWarning("Delete requested for missing teacher {Id}", id); return Error<object>(404, "TEACHER_NOT_FOUND", "Teacher not found."); }
        try { await _repo.DeleteAsync(entity); _logger.LogInformation("Deleted teacher {Id}", id); return OkResult(new { message = "Teacher deleted." }); }
        catch (DbUpdateException) { return Error<object>(409, "TEACHER_HAS_COURSES", "Cannot delete a teacher who still has courses."); }
    }
}
