using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.PetManagement.Entities.ValueObjects;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.UseCases.CreateVolunteer;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator() 
    {
        RuleFor(c => c.Description).MustBeValueObject(NotNullableText.Create);
        RuleFor(c => new {c.FirstName, c.LastName, c.Patronymic})
            .MustBeValueObject(f => 
                FullName.Create(f.FirstName, f.LastName, f.Patronymic));
        RuleFor(c => c.Experience).MustBeValueObject(Experience.Create);
        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);


        RuleForEach(c => c.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Title, r.Description));
        RuleForEach(c => c.SocialNetworks)
            .MustBeValueObject(s => SocialNetwork.Create(s.Title, s.Link));
    }
}