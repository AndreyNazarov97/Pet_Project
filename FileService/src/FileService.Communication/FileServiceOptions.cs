namespace FileService.Communication;

public class FileServiceOptions
{
    public const string FileService = nameof(FileService);
    
    public string Url { get; set; } = string.Empty;
}