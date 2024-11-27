using FluentValidation;
using PetProject.Core.Dtos.Validators;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.UpdateSocialLinks;

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