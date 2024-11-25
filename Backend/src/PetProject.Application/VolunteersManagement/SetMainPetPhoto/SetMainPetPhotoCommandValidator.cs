using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.VolunteersManagement.SetMainPetPhoto;

public class SetMainPetPhotoCommandValidator : AbstractValidator<SetMainPetPhotoCommand>
{
    public SetMainPetPhotoCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("VolunteerId"));
        
        RuleFor(u => u.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("PetId"));

        RuleFor(s => new { s.Path })
            .MustBeValueObject(x =>
            {
                var extension = Path.GetExtension(x.Path);
                var path = Path.GetFileNameWithoutExtension(x.Path);
                return FilePath.Create(path, extension);
            });
    }
}