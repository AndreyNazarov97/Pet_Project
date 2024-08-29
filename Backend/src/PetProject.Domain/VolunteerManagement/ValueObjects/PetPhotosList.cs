namespace PetProject.Domain.VolunteerManagement.ValueObjects;

public record PetPhotosList
{
    private PetPhotosList(){}
    public IReadOnlyCollection<PetPhoto> PetPhotos { get; }
    public PetPhotosList(IEnumerable<PetPhoto> petPhotos) => PetPhotos = petPhotos.ToList();
}