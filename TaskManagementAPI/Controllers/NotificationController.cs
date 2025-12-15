using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Services.Interfaces;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);
            return Ok(await _service.GetMyNotificationsAsync(userId));
        }

        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _service.MarkAsReadAsync(id);
            return Ok();
        }
    }


}
