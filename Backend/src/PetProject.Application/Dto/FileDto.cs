namespace PetProject.Application.Dto;

public record FileDto(string FileName, string ContentType, Stream Content);