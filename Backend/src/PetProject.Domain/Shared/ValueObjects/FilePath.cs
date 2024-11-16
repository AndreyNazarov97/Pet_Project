using CSharpFunctionalExtensions;

namespace PetProject.Domain.Shared.ValueObjects;

public record FilePath
{
    public string Path { get; }

    private FilePath(string path)
    {
        Path = path;
    }

    public static Result<FilePath, Error> Create(string path, string extension)
    {
        if (!Constants.Extensions.Contains(extension))
            return Errors.General.ValueIsInvalid("extension");
        
        if(string.IsNullOrWhiteSpace(path) || path.Length > Constants.MIDDLE_TEXT_LENGTH)
            return Errors.General.LengthIsInvalid(path);

        var fullPath = path + extension; 
        
        return new FilePath(fullPath);
    }
}