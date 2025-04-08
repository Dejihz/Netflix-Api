using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project6._1Api.Entities;
using project6._1Api.Model;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class LoginController : ControllerBase
{
    private readonly JwtService _jwtService;

    public LoginController(JwtService jwtService) => _jwtService = jwtService;

    [HttpPost("")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var result = await _jwtService.Authenticate(request);
        if (result is null)
            return Unauthorized();

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<LoginResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _jwtService.RefreshToken(request.RefreshToken);
        if (result is null)
            return Unauthorized();

        return Ok(result);
    }
}