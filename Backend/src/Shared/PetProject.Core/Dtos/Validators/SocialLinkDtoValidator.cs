using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Core.Dtos.Validators;

public class SocialLinkDtoValidator : AbstractValidator<SocialNetworkDto>
{
    public SocialLinkDtoValidator()
    {
        RuleFor(c => new { Name = c.Title, c.Url})
            .MustBeValueObject(s => SocialNetwork.Create(s.Name, s.Url));
    }
}