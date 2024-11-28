using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.SoftDeletePet;

public class SoftDeletePetCommandValidator : AbstractValidator<SoftDeletePetCommand>
{
    public SoftDeletePetCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("VolunteerId"));
        
        RuleFor(u => u.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("PetId"));
    }
}