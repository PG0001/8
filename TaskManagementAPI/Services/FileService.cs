using Library8.Models;
using Library8.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Services.Interfaces;
namespace TaskManagementAPI.Services
{

    public class FileService : IFileService
    {
        private readonly IFileRepository _repo;
        private readonly IWebHostEnvironment _env;

        public FileService(IFileRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        public async Task<FileResponseDto> UploadForTaskAsync(
            int taskId, IFormFile file, int userId)
        {
            return await UploadAsync(taskId, "Task", file, userId);
        }

        public async Task<FileResponseDto> UploadForProjectAsync(
            int projectId, IFormFile file, int userId)
        {
            return await UploadAsync(projectId, "Project", file, userId);
        }

        public async Task<IEnumerable<FileResponseDto>> GetFilesAsync(
            int entityId, string entityType)
        {
            var files = await _repo.GetFilesByEntityAsync(entityId, entityType);

            return files.Select(f => new FileResponseDto
            {
                Id = f.Id,
                FileName = f.FileName,
                FilePath = f.FilePath,
                UploadedOn = f.UploadedOn
            });
        }

        // ---------------- PRIVATE ----------------

        private async Task<FileResponseDto> UploadAsync(
            int entityId,
            string entityType,
            IFormFile file,
            int userId)
        {
            var uploadsRoot = Path.Combine(
                _env.ContentRootPath, "Uploads", entityType);

            if (!Directory.Exists(uploadsRoot))
                Directory.CreateDirectory(uploadsRoot);

            var uniqueFileName =
                $"{Guid.NewGuid()}_{file.FileName}";

            var filePath = Path.Combine(uploadsRoot, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var entity = new Library8.Models.File
            {
                FileName = file.FileName,
                FilePath = filePath,
                UploadedBy = userId,
                RelatedEntityId = entityId,
                EntityType = entityType,
                UploadedOn = DateTime.UtcNow
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();

            return new FileResponseDto
            {
                Id = entity.Id,
                FileName = entity.FileName,
                FilePath = entity.FilePath,
                UploadedOn = entity.UploadedOn
            };
        }
    }
}
