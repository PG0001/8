namespace TaskManagementAPI.Services.Interfaces
{
    using TaskManagementAPI.Dtos;

    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetSummaryAsync();
    }

}
