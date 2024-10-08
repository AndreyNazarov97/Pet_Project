﻿using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Volunteers.CreateVolunteer;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(c => new { c.FullName.Name, c.FullName.Surname, c.FullName.Patronymic })
            .MustBeValueObject(x => FullName.Create(x.Name, x.Surname, x.Patronymic));

        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(c => c.AgeExperience)
            .MustBeValueObject(Experience.Create);

        RuleFor(c => c.Number)
            .MustBeValueObject(PhoneNumber.Create);

        RuleForEach(c => c.Requisites)
            .MustBeValueObject(s => Requisite.Create(s.Name, s.Description));

        RuleForEach(c => c.SocialLinks)
            .MustBeValueObject(s => SocialLink.Create(s.Name, s.Url));
    }
}