namespace PetProject.Domain.VolunteerManagement.ValueObjects;

public record PetPhotosList
{
    private readonly List<PetPhoto> _petPhotos = [];
    private PetPhotosList(){}
    public IReadOnlyCollection<PetPhoto> PetPhotos => _petPhotos.AsReadOnly();
    public PetPhotosList(IEnumerable<PetPhoto> petPhotos) => _petPhotos = petPhotos.ToList();
    
    public void AddPhotos(IEnumerable<PetPhoto> petPhotos) => _petPhotos.AddRange(petPhotos);
}