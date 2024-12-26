using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;
using VolunteerRequests.Domain.ValueObjects;

namespace VolunteerRequests.Application.RequestsManagement.Commands.SendForRevision;

public class SendForRevisionCommandValidator : AbstractValidator<SendForRevisionCommand>
{
    public SendForRevisionCommandValidator()
    {
        RuleFor(x => x.VolunteerRequestId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
        
        RuleFor(x => x.AdminId)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(x => x.RejectionComment)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
    }
}