using FluentValidation;
using PetProject.Application.Dto;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.Volunteers.UpdateVolunteer;

public class UpdateVolunteerRequestValidator : AbstractValidator<UpdateVolunteerRequest>
{
    public UpdateVolunteerRequestValidator()
    {
        RuleFor(v => v.IdVolunteer)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("Id"));;
        
    }
}

public class UpdateVolunteerDtoValidator : AbstractValidator<UpdateVolunteerDto>
{
    public UpdateVolunteerDtoValidator()
    {
        RuleFor(c => new { c.FullName.Name, c.FullName.Surname, c.FullName.Patronymic })
            .MustBeValueObject(x => FullName.Create(x.Name, x.Surname, x.Patronymic));

        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(c => c.AgeExperience)
            .MustBeValueObject(Experience.Create);

        RuleFor(c => c.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
    }
}