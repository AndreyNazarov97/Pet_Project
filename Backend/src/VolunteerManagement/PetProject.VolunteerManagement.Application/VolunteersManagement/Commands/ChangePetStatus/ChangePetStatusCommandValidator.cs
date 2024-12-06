using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.ChangePetStatus;

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