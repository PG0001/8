namespace TaskManagementAPI.Hubs
{
    using Library8.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly DBContext _context;

        public ChatHub(DBContext context)
        {
            _context = context;
        }

        public async Task JoinProjectChat(int projectId)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                $"chat-{projectId}"
            );
        }

        public async Task SendMessage(int projectId, string message)
        {
            // 🔐 Get user info from JWT
            var userIdClaim = Context.User?.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new HubException("Unauthorized");

            var userId = int.Parse(userIdClaim);
            var userName = Context.User?.Identity?.Name ?? "Unknown";

            // 💾 Save message to DB
            var chat = new ProjectChat
            {
                ProjectId = projectId,
                UserId = userId,
                Message = message,
                SentOn = DateTime.UtcNow
            };

            _context.ProjectChats.Add(chat);
            await _context.SaveChangesAsync();

            // 📡 Broadcast message
            await Clients.Group($"chat-{projectId}")
                .SendAsync("ReceiveMessage", new
                {
                    user = userName,
                    message = message,
                    sentOn = chat.SentOn
                });
        }
    }
}
