using AutoMapper;
using BusinessObjects.DTOs.Request;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace eStore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, IMapper mapper) : ControllerBase
{
    private IAuthService AuthService { get; } = authService;
    private IMapper Mapper { get; } = mapper;

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Email))
        {
            return BadRequest(new { message = "Email address needs to entered" });
        }

        if (string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { message = "Password needs to entered" });
        }

        var user = await AuthService.Login(request.Email, request.Password);
        if (user == null)
        {
            return BadRequest("Invalid email or password");
        }
        return Ok(new { message = "User login successful" });
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Email))
        {
            return BadRequest(new { message = "Email address needs to entered" });
        }

        if (string.IsNullOrEmpty(request.Password))
        {
            return BadRequest(new { message = "Password needs to entered" });
        }

        if (string.IsNullOrEmpty(request.CompanyName))
        {
            return BadRequest(new { message = "Company name needs to entered" });
        }
        var member = Mapper.Map<Member>(request);
        var registeredMember = await AuthService.Register(member);
        if (member == null)
        {
            return BadRequest("User registration failed");
        }
        return Ok(new { message = "User registration successful" });
    }
}