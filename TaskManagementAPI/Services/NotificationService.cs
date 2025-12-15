using Library8.Models;
using Library8.Models.Interfaces;
using Microsoft.AspNetCore.SignalR;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Hubs;
using TaskManagementAPI.Services.Interfaces;

namespace TaskManagementAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly IHubContext<Hubs.NotificationHub> _hubContext;

        public NotificationService(
        INotificationRepository repo,
        IHubContext<NotificationHub> hubContext)
        {
            _repo = repo;
            _hubContext = hubContext;
        }

        public async Task NotifyAsync(
        int userId,
        string title,
        string message,
        string type)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                IsRead = false
            };

            await _repo.AddAsync(notification);
            await _repo.SaveChangesAsync();

            await _hubContext.Clients
                .Group($"user-{userId}")
                .SendAsync("NotificationReceived", new
                {
                    notification.Id,
                    notification.Title,
                    notification.Message,
                    notification.CreatedOn
                });
        }


        public async Task<IEnumerable<NotificationResponseDto>>
            GetMyNotificationsAsync(int userId)
        {
            var data = await _repo.GetByUserAsync(userId);

            return data.Select(n => new NotificationResponseDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedOn = n.CreatedOn
            });
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _repo.GetByIdAsync(notificationId);
            if (notification == null) return;

            notification.IsRead = true;
            await _repo.SaveChangesAsync();
        }
    }

}
