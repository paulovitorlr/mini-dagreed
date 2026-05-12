using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using user_service.DTOs;
using user_service.Services;

namespace user_service.Controllers;

[ApiController]
[Route("users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpPost("internal/create")]
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateUserDTO dto)
    {
        var user = await _service.CreateAsync(dto);
        return Ok(User);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _service.GetByAuthIdAsync(authId);
        if (user == null) return NotFound("Perfil não encontrado.");
        return Ok(user);
    }

    [HttpPut("me")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDTO dto)
    {
        var authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _service.UpdateAsync(authId, dto);
        if (user == null) return NotFound("Perfil não encontrado.");
        return Ok(user);
    }
}

