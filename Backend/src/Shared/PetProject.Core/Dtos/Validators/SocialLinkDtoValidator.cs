using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Core.Dtos.Validators;

public class SocialLinkDtoValidator : AbstractValidator<SocialLinkDto>
{
    public SocialLinkDtoValidator()
    {
        RuleFor(c => new { Name = c.Title, c.Url})
            .MustBeValueObject(s => SocialLink.Create(s.Name, s.Url));
    }
}