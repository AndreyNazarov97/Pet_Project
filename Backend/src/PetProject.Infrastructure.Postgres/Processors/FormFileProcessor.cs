using Microsoft.AspNetCore.Http;
using PetProject.Application.Dto;

namespace PetProject.Infrastructure.Postgres.Processors;

public class FormFileProcessor: IAsyncDisposable
{
    private readonly List<FileDto> _filesDto = [];

    public List<FileDto> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            var fileDto = new FileDto(file.FileName, file.ContentType, stream);
            _filesDto.Add(fileDto);
        }

        return _filesDto;
    }
    
    
    public async ValueTask DisposeAsync()
    {
        foreach (var file in _filesDto)
        {
            await file.Content.DisposeAsync();
        }
    }
}