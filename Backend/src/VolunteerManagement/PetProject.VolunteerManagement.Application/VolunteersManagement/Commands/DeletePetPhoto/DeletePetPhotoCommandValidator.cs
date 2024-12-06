using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.DeletePetPhoto;

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