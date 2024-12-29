using FluentValidation;
using PetProject.Core.Validation;
using PetProject.Discussions.Domain.ValueObjects;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Application.DiscussionsManagement.Commands.SendMessage;

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid("User id"));

        RuleFor(x => x.DiscussionId)
            .NotNull()
            .WithError(Errors.General.Null());

        RuleFor(x => x.Text)
            .MustBeValueObject(Text.Create);
    }
}