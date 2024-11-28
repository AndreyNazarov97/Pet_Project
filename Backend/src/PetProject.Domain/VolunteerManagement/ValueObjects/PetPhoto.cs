using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Domain.VolunteerManagement.ValueObjects;

public record PetPhoto
{
    public FilePath Path { get; }
    public bool IsMain { get; init; }

    private PetPhoto()
    {
        
    }
    public PetPhoto(FilePath path)
    {
        Path = path;
    }
}