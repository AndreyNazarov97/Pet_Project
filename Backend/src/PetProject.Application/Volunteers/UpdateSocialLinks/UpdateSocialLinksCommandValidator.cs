using FluentValidation;
using PetProject.Application.Dto;
using PetProject.Application.Dto.Validators;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Volunteers.UpdateSocialLinks;

public class UpdateSocialLinksCommandValidator : AbstractValidator<UpdateSocialLinksCommand>
{
    public UpdateSocialLinksCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithError(Errors.General.LengthIsInvalid("Id"));
        
        RuleForEach(x => x.SocialLinks)
            .SetValidator(new SocialLinkDtoValidator());
    }
    
}