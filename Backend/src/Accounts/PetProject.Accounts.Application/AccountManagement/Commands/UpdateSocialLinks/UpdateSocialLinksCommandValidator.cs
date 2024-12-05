using FluentValidation;
using PetProject.Core.Dtos.Validators;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Commands.UpdateSocialLinks;

public class UpdateSocialLinksCommandValidator : AbstractValidator<UpdateSocialLinksCommand>
{
    public UpdateSocialLinksCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithError(Errors.General.LengthIsInvalid("Id"));
        
        RuleForEach(x => x.SocialLinks)
            .SetValidator(new SocialLinkDtoValidator());
    }
    
}