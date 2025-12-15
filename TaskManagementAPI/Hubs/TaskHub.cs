namespace TaskManagementAPI.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class TaskHub : Hub
    {
        public async Task JoinProject(int projectId)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                $"project-{projectId}"
            );
        }

        public async Task LeaveProject(int projectId)
        {
            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId,
                $"project-{projectId}"
            );
        }
    }

}
