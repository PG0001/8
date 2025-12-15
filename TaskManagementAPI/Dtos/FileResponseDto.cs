namespace TaskManagementAPI.Dtos
{
    public class FileResponseDto
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public DateTime UploadedOn { get; set; }
    }

}
