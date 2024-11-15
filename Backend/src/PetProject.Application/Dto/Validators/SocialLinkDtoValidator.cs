using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Dto.Validators;

public class SocialLinkDtoValidator : AbstractValidator<SocialLinkDto>
{
    public SocialLinkDtoValidator()
    {
        RuleFor(c => new {c.Name, c.Url})
            .MustBeValueObject(s => Requisite.Create(s.Name, s.Url));
    }
}