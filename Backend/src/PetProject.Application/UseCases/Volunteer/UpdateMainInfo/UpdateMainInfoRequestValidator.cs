using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.PetManagement.Entities.ValueObjects;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.UseCases.Volunteer.UpdateMainInfo;

public class UpdateMainInfoRequestValidator : AbstractValidator<UpdateMainInfoRequest>
{
    public UpdateMainInfoRequestValidator()
    {
        
        RuleFor(c => c.Dto.Description).MustBeValueObject(NotNullableText.Create);
        RuleFor(c => new {c.Dto.FirstName, c.Dto.LastName, c.Dto.Patronymic})
            .MustBeValueObject(f => 
                FullName.Create(f.FirstName, f.LastName, f.Patronymic));
        RuleFor(c => c.Dto.Experience).MustBeValueObject(Experience.Create);
        RuleFor(c => c.Dto.PhoneNumber).MustBeValueObject(PhoneNumber.Create);
    }
}