using auth_service.DTOs;
using auth_service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth_service.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    //Este controller depende de um serviço que implemente IAuthService.
    //O ASP.NET Core vai fornecer essa implementação automaticamente via Injeção de Dependência.
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var result = await _service.RegisterAsync(dto);
        if (!result) return BadRequest("Email já cadastrado.");

        return Ok("Usuário registrado com sucesso!");
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var token = await _service.LoginAsync(dto);
        if (token == null) return Unauthorized("Email ou senha invalidos.");
        return Ok(new { token });
    }

    [HttpGet("validate")]
    public async Task<IActionResult> Validate([FromHeader] string authorization)
    {
        var token = authorization.Replace("Bearer ", "");
        var result = await _service.ValidateAsync(token);
        if (!result) return Unauthorized("Token invalido.");
        return Ok("Token válido");
    }


}

