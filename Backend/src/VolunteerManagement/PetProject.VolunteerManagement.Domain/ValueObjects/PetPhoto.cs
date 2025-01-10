using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.VolunteerManagement.Domain.ValueObjects;

public record PetPhoto
{
    //TODO: здесь должно быть FileId, а не Path
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