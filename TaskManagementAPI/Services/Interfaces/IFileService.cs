using TaskManagementAPI.Dtos;

namespace TaskManagementAPI.Services.Interfaces
{
    public interface IFileService
    {
        Task<FileResponseDto> UploadForTaskAsync(
        int taskId, IFormFile file, int userId);

        Task<FileResponseDto> UploadForProjectAsync(
            int projectId, IFormFile file, int userId);

        Task<IEnumerable<FileResponseDto>> GetFilesAsync(
            int entityId, string entityType);
    }
}
