using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.VolunteersManagement.DeletePetPhoto;

public class DeletePetPhotoCommandValidator : AbstractValidator<DeletePetPhotoCommand>
{
    public DeletePetPhotoCommandValidator()
    {
        RuleFor(d => d.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("VolunteerId"));
        
        RuleFor(d => d.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("PetId"));

        RuleFor(d => d.FilePath)
            .MustBeValueObject(FilePath.Create);
    }
}