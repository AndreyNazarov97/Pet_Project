using FluentValidation;
using PetProject.Application.Dto.Validators;
using PetProject.Application.Validation;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.VolunteersManagement.CreateVolunteer;

public class CreateVolunteerCommandValidator : AbstractValidator<CreateVolunteerCommand>
{
    public CreateVolunteerCommandValidator()
    {
        RuleFor(c => c.FullName)
            .SetValidator(new FullNameDtoValidator());
        
        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(c => c.AgeExperience)
            .MustBeValueObject(Experience.Create);

        RuleFor(c => c.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);

        RuleForEach(c => c.Requisites)
            .SetValidator(new RequisiteDtoValidator()); 

        RuleForEach(c => c.SocialLinks)
            .SetValidator(new SocialLinkDtoValidator());
    }
}