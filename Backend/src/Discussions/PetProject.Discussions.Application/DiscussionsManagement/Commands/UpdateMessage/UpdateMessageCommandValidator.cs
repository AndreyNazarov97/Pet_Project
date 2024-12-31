using FluentValidation;
using PetProject.Core.Validation;
using PetProject.Discussions.Application.DiscussionsManagement.Commands.SendMessage;
using PetProject.Discussions.Domain.ValueObjects;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Application.DiscussionsManagement.Commands.UpdateMessage;

public class UpdateMessageCommandValidator : AbstractValidator<UpdateMessageCommand>
{
    public UpdateMessageCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid("User id"));

        RuleFor(x => x.DiscussionId)
            .NotNull()
            .WithError(Errors.General.Null());
        
        RuleFor(x => x.MessageId)
            .NotNull()
            .WithError(Errors.General.Null());

        RuleFor(x => x.Text)
            .MustBeValueObject(Text.Create);
    }
}