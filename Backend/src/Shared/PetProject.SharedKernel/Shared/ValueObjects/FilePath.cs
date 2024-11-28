using CSharpFunctionalExtensions;

namespace PetProject.SharedKernel.Shared.ValueObjects;

public record FilePath
{
    public string Path { get; }
    
    private FilePath() {}

    private FilePath(string path)
    {
        Path = path;
    }

    public static Result<FilePath, Error> Create(string path, string extension)
    {
        if (!Constants.Constants.Extensions.Contains(extension))
            return Errors.General.ValueIsInvalid("extension");
        
        if(string.IsNullOrWhiteSpace(path) || path.Length > Constants.Constants.MIDDLE_TEXT_LENGTH)
            return Errors.General.LengthIsInvalid(path);

        var fullPath = path + extension; 
        
        return new FilePath(fullPath);
    }
    
    public static Result<FilePath, Error> Create(string fullPath)
    {
        var extension = System.IO.Path.GetExtension(fullPath);
        var fileName = System.IO.Path.GetFileNameWithoutExtension(fullPath);

        return Create(fileName, extension);
    }
}