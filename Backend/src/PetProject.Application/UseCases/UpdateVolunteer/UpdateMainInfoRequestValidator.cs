using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.PetManagement.Entities.ValueObjects;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.UseCases.UpdateVolunteer;

public class UpdateMainInfoRequestValidator : AbstractValidator<UpdateMainInfoRequest>
{
    public UpdateMainInfoRequestValidator()
    {
        RuleFor(c => c.Description).MustBeValueObject(NotNullableText.Create);
        RuleFor(c => new {c.FirstName, c.LastName, c.Patronymic})
            .MustBeValueObject(f => 
                FullName.Create(f.FirstName, f.LastName, f.Patronymic));
        RuleFor(c => c.Experience).MustBeValueObject(Experience.Create);
        RuleFor(c => c.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
    }
}