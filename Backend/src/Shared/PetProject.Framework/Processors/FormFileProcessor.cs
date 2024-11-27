using Microsoft.AspNetCore.Http;
using PetProject.Core.Dtos;

namespace PetProject.Framework.Processors;

public class FormFileProcessor: IAsyncDisposable
{
    private readonly List<FileDto> _filesDto = [];

    public List<FileDto> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            var fileDto = new FileDto
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Content = stream
            };
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