using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.VolunteerManagement.Domain.ValueObjects;

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