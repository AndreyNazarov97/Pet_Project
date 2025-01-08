namespace FileService.Communication.Contracts.Requests;

public record GetFilesByIdsRequest(IEnumerable<Guid> FileIds);