using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Domain.VolunteerManagement.ValueObjects;

public record PetPhoto
{
    public FilePath Path { get; }
    public bool IsMain { get; } = false;

    public PetPhoto(FilePath path)
    {
        Path = path;
    }
}