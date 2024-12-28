using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;

namespace VolunteerRequests.Application.RequestsManagement.Commands.Reject;

public class RejectVolunteerRequestCommandValidator : AbstractValidator<RejectVolunteerRequestCommand>
{
    public RejectVolunteerRequestCommandValidator()
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