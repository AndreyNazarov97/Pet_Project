namespace FileService.Communication.Contracts.Responses;

public record GetFilesByIdsResponse(IEnumerable<FileInfo> FilesInfo);

public record FileInfo(Guid Id, string DownloadUrl, string Key, DateTime UploadDate);