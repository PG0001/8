using TaskManagementAPI.Dtos;

namespace TaskManagementAPI.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotifyAsync(int userId, string title, string message, string type);
        Task<IEnumerable<NotificationResponseDto>> GetMyNotificationsAsync(int userId);
        Task MarkAsReadAsync(int notificationId);
    }

}
