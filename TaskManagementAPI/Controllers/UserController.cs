using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Services.Interfaces;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(
    [FromBody] UpdateProfileDto dto)
        {
            var userId = int.Parse(
                User.FindFirst("UserId")!.Value);

            var updated = await _userService.UpdateProfileAsync(
                userId, dto);

            if (!updated)
                return BadRequest("Unable to update profile.");

            return Ok(new { Message = "Profile updated successfully." });
        }

        // GET CURRENT USER
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (email == null) return Unauthorized();

            var user = await _userService.GetUserByEmailAsync(email);
            return Ok(user);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUser = await _userService.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest("User with this email already exists.");

            var newUser = await _userService.RegisterUserAsync(request.FullName,request.Email,request.Password,request.Role);
            return Ok(newUser);
        }
    }
}
