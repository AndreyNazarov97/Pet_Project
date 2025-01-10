namespace FileService.Communication.Contracts.Requests;

public record DeleteFilesRequest(IEnumerable<Guid> FileIds);