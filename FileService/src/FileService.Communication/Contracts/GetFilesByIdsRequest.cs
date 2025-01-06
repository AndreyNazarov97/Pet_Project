namespace FileService.Communication.Contracts;

public record GetFilesByIdsRequest(IEnumerable<Guid> FileIds);