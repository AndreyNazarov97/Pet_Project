using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Application.DiscussionsManagement.Commands;

public class CreateDiscussionCommandValidator : AbstractValidator<CreateDiscussionCommand>
{
    public CreateDiscussionCommandValidator()
    {
        RuleFor(x => x.RelationId)
            .NotNull()
            .WithError(Errors.General.Null());

        RuleFor(x => x.FirstMemberId)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid());

        RuleFor(x => x.SecondMemberId)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid());
    }
}