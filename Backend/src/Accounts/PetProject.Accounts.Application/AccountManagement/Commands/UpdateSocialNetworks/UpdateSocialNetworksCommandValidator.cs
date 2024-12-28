using FluentValidation;
using PetProject.Core.Dtos.Validators;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Commands.UpdateSocialNetworks;

public class UpdateSocialNetworksCommandValidator : AbstractValidator<UpdateSocialNetworksCommand>
{
    public UpdateSocialNetworksCommandValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithError(Errors.General.LengthIsInvalid("Id"));
        
        RuleForEach(x => x.SocialLinks)
            .SetValidator(new SocialNetworkDtoValidator());
    }
    
}