using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;

namespace PetProject.Domain.VolunteerManagement.ValueObjects;

public record PetPhotosList
{
    private readonly List<PetPhoto> _petPhotos = [];

    private PetPhotosList()
    {
    }

    public IReadOnlyCollection<PetPhoto> PetPhotos => _petPhotos.AsReadOnly();
    public PetPhotosList(IEnumerable<PetPhoto> petPhotos) => _petPhotos = petPhotos.ToList();

    public void AddPhotos(IEnumerable<PetPhoto> petPhotos) => _petPhotos.AddRange(petPhotos);

    public UnitResult<Error> SetMainPhoto(PetPhoto petPhoto)
    {
        // TODO тут опять ошибка с PetPhoto.Id несуществующим
        // Надо думать как обновить флаги у остальных фото
        var mainPetPhoto = _petPhotos.FirstOrDefault(p => p.Path == petPhoto.Path);
        if (mainPetPhoto is null)
            return Errors.General.NotFound();

        _petPhotos.Remove(mainPetPhoto);

        for (var i = 0; i < _petPhotos.Count; i++)
        {
            var currentPhoto = _petPhotos[i];
            if (currentPhoto.IsMain)
            {
                _petPhotos[i] = currentPhoto with { IsMain = false };
            }
        }

        _petPhotos.Insert(0, mainPetPhoto with { IsMain = true });


        return UnitResult.Success<Error>();
    }
}