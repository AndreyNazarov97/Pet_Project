using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Dto.Validators;

public class SocialLinkDtoValidator : AbstractValidator<SocialLinkDto>
{
    public SocialLinkDtoValidator()
    {
        RuleFor(c => new { Name = c.Title, c.Url})
            .MustBeValueObject(s => SocialLink.Create(s.Name, s.Url));
    }
}