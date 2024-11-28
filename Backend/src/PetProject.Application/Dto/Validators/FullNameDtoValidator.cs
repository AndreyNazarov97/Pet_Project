using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.Dto.Validators;

public class FullNameDtoValidator : AbstractValidator<FullNameDto>
{
    public FullNameDtoValidator()
    {
        RuleFor(c => new { c.Name, c.Surname, c.Patronymic })
            .MustBeValueObject(x => FullName.Create(x.Name, x.Surname, x.Patronymic));
    }
}