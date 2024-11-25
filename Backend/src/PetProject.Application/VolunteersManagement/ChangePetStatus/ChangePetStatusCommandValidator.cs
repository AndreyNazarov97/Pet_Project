using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.ChangePetStatus;

public class ChangePetStatusCommandValidator : AbstractValidator<ChangePetStatusCommand>
{
    public ChangePetStatusCommandValidator()
    {
        RuleFor(u => u.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("VolunteerId"));
        
        RuleFor(u => u.PetId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid("PetId"));
    }
}