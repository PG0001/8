using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Services.Interfaces;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;   // Handles login & register

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // ---------------- LOGIN ----------------
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(request);
        if (result == null)
            return Unauthorized("Invalid email or password.");

        return Ok(result); // returns AuthResponseDto
    }

    // ---------------- REGISTER ----------------
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await _authService.GetUserByEmailAsync(request.Email);
        if (existingUser != null)
            return BadRequest("User with this email already exists.");

        var newUser = await _authService.RegisterAsync(
            request.FullName,
            request.Email,
            request.Password,
            request.Role
        );

        return Ok(new AuthResponseDto
        {
            Token = newUser.Token,   // JWT generated after registration
            Email = newUser.Email,
            Role = newUser.Role
        });
    }
    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(
    [FromBody] ChangePasswordDto dto)
    {
        var email = User.FindFirst(
            System.Security.Claims.ClaimTypes.Email)?.Value;

        if (email == null) return Unauthorized();

        var success = await _authService.ChangePasswordAsync(
            email, dto.OldPassword, dto.NewPassword);

        if (!success)
            return BadRequest("Old password is incorrect.");

        return Ok(new { Message = "Password updated successfully." });
    }

}
