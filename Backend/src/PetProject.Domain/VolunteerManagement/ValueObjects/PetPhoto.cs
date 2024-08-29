using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;

namespace PetProject.Domain.VolunteerManagement.ValueObjects;

public record PetPhoto
{
    public string Path { get; }
    public bool IsMain { get; }

    private PetPhoto(string path, bool isMain)
    {
        Path = path;
        IsMain = isMain;
    }

    public static Result<PetPhoto, Error> Create(string path, bool isImageMain)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Errors.General.ValueIsInvalid("path cannot be empty");

        return new PetPhoto(path, isImageMain);
    }
}