using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Services.Interfaces;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/files")]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileService _service;

        public FileController(IFileService service)
        {
            _service = service;
        }

        // ---------- TASK FILES ----------
        [HttpPost("task/{taskId}")]
        public async Task<IActionResult> UploadTaskFile(
            int taskId, IFormFile file)
        {
            var userIdClaim = User.FindFirst("UserId");

            if (userIdClaim == null)
                return Unauthorized("UserId claim missing");

            var userId = int.Parse(userIdClaim.Value);

            var result = await _service.UploadForTaskAsync(
                taskId, file, userId);

            return Ok(result);
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetTaskFiles(int taskId)
        {
            var files = await _service.GetFilesAsync(taskId, "Task");
            return Ok(files);
        }

        // ---------- PROJECT FILES ----------
        [HttpPost("project/{projectId}")]
        public async Task<IActionResult> UploadProjectFile(
            int projectId, IFormFile file)
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);

            var result = await _service.UploadForProjectAsync(
                projectId, file, userId);

            return Ok(result);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetProjectFiles(int projectId)
        {
            var files = await _service.GetFilesAsync(projectId, "Project");
            return Ok(files);
        }
    }
}
