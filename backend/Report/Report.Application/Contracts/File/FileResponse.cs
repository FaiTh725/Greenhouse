namespace Report.Application.Contracts.File
{
    public class FileResponse
    {
        public required Stream Stream { get; set; }

        public required string ContentType { get; set; }

        public string FileName { get; set; } = string.Empty;
    }
}
