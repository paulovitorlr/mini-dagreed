using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using recommendation_service.Services;

namespace recommendation_service.Controllers;

[ApiController]
[Route("recommendations")]
[Authorize]
public class RecommendationController : ControllerBase
{
    private readonly IRecommendationService _service;

    public RecommendationController(IRecommendationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecommendations()
    {
        var authId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var result = await _service.GetRecommendationsAsync(authId, token);
        return Ok(result);
    }
}

