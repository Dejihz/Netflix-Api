using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project6._1Api.Entities;
using project6._1Api.Model;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
[Produces("application/json", "application/xml")]
[Consumes("application/json", "application/xml")]
public class LoginController : ControllerBase
{
    private readonly JwtService _jwtService;

    public LoginController(JwtService jwtService) => _jwtService = jwtService;

    [HttpPost("")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var result = await _jwtService.Authenticate(request);
        if (result is null)
        {
            return Unauthorized();
        }

        return Ok(result);
    }
}