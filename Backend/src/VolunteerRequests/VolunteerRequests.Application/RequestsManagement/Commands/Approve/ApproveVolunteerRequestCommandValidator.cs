using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;

namespace VolunteerRequests.Application.RequestsManagement.Commands.Approve;

public class ApproveVolunteerRequestCommandValidator : AbstractValidator<ApproveVolunteerRequestCommand>
{
    public ApproveVolunteerRequestCommandValidator()
    {
        RuleFor(x => x.VolunteerRequestId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
        
        RuleFor(x => x.AdminId)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid());
  
    }
}