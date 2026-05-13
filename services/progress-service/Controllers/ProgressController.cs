using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using progress_service.DTOs;
using progress_service.Services;

namespace progress_service.Controllers;

[ApiController]
[Route("progress")]
[Authorize]
public class ProgressController : ControllerBase 
{
    private readonly IProgressService _service;

    public ProgressController(IProgressService service)
    {
        _service = service;
    }

    [HttpPost("enroll")]
    public async Task<IActionResult> Enroll([FromBody] EnrollDTO dto)
    {
        var authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _service.EnrollAsync(authId, dto);
        if (result == null) return BadRequest("Usuário já matriculado neste curso.");
        return Ok(result);
    }

    [HttpPost("complete-lesson")]
    public async Task<IActionResult> CompleteLesson([FromBody] CompleteLessonDTO dto)
    {
        var authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _service.CompleteLessonAsync(authId, dto);
        if (result == null) return NotFound("Matrícula não encontrada.");
        return Ok(result);
    }

    [HttpGet("my-progress")]
    public async Task<IActionResult> GetMyProgress()
    {
        var authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _service.GetAllByAuthIdAsync(authId);
        return Ok(result);
    }
}

