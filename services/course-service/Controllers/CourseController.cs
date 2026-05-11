using course_service.DTOs;
using course_service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace course_service.Controllers;

[ApiController]
[Route("courses")]
[Authorize]
public class CourseController : ControllerBase
{
    private readonly ICourseService _service;

    public CourseController(ICourseService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]

    public async Task<IActionResult> GetAll()
    {
        var courses = await _service.GetAllAsync();
        return Ok(courses);
    }
    public async Task<IActionResult> GetById(int id)
    {
        var course = await _service.GetByIdAsync(id);
        if (course == null) return NotFound("Curso não encontrado.");
        return Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCourseDTO dto)
    {
        var course = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDTO dot)
    {
        var course = await _service.UpdateAsync(id, dot);
        if (course == null) return NotFound("Curso não encontrado.");
        return Ok(course);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
       var result = await _service.DeleteAsync(id);
        if (!result) return NotFound("Curso não encontrado.");
        return NoContent();
    }

    
}

