using FluentValidation;
using PetProject.Application.Dto.Validators;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.UpdateSocialLinks;

public class UpdateSocialLinksCommandValidator : AbstractValidator<UpdateSocialLinksCommand>
{
    public UpdateSocialLinksCommandValidator()
    {
        RuleFor(x => x.VolunteerId)
            .NotEmpty()
            .WithError(Errors.General.LengthIsInvalid("Id"));
        
        RuleForEach(x => x.SocialLinks)
            .SetValidator(new SocialLinkDtoValidator());
    }
    
}