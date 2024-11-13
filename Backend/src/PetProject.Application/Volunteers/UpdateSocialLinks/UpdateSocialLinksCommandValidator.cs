using FluentValidation;
using PetProject.Application.Dto;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Volunteers.UpdateSocialLinks;

public class UpdateSocialLinksRequestValidator : AbstractValidator<UpdateSocialLinksCommand>
{
    public UpdateSocialLinksRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("Id"));
    }
}

public class UpdateSocialLinksDtoValidator : AbstractValidator<UpdateSocialLinksDto>
{
    public UpdateSocialLinksDtoValidator()
    {
        RuleForEach(c => c.SocialLinks)
            .MustBeValueObject(s => Requisite.Create(s.Name, s.Url));
    }
}